namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using AngleSharp.Dom;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models;

    using static Constants.ServicesConstants;

    public class Q88ScraperService : IQ88ScraperService
    {      
        private readonly IConfiguration config;
        private readonly IBrowsingContext browsingContext;
        private readonly VesselInfoDbContext context;

        public Q88ScraperService(VesselInfoDbContext context)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(this.config);
            this.context = context;
        }

        public async Task ImportVesselDataAsync(char fromId = START_LETTER, char toId = END_LETTER)
        {         
            var vessels = this.GetVesselListing(fromId, toId);

            if (vessels.Any())
            {
                var names = vessels[0].Name;
                var summerDwts = vessels[0].SummerDwt;
                var loas = vessels[0].Loa;
                var imos = vessels[0].Imo;
                var hullTypes = vessels[0].HullType;
                var cubic = vessels[0].Cubic;
                var beams = vessels[0].Beam;
                var drafts = vessels[0].Draft;
                var callSigns = vessels[0].CallSign;
                var builts = vessels[0].Built;

                var guids = await this.ScrapeGuids(this.browsingContext, fromId);
                var owners = await this.ScrapeOwners(this.browsingContext, guids);
                var types = await this.ScrapeTypes(this.browsingContext, guids);
                var classSocieties = await this.ScrapeClassSocieties(this.browsingContext, guids);
                var registrations = await this.ScrapeRegistrationsWithPorts(this.browsingContext, guids);

                // Size of each entity. They should always be equal.
                var vesselSize = names.Count;

                for (int i = 0; i < vesselSize; i++)
                {
                    var typeId = this.GetOrCreateType(types[i]);
                    var ownerId = this.GetOrCreateOwner(owners[i]);
                    var classSocietyId = this.GetOrCreateClassSociety(classSocieties[i]);

                    var registrationKeys = registrations[i].Keys.FirstOrDefault();
                    var registrationValues = registrations[i].Values.FirstOrDefault();
                    var registrationId = this.GetOrCreateRegistration(registrationKeys, registrationValues);

                    var newVessel = new Vessel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = names[i],
                        Imo = imos[i],
                        Built = builts[i],
                        SummerDwt = summerDwts[i],
                        Loa = loas[i],
                        Cubic = cubic[i],
                        Beam = beams[i],
                        Draft = drafts[i],
                        HullType = hullTypes[i],
                        CallSign = callSigns[i],
                        TypeId = typeId,
                        OwnerId = ownerId,
                        RegistrationId = registrationId,
                        ClassificationSocietyId = classSocietyId
                    };

                    await this.context.Vessels.AddAsync(newVessel);
                }

                await this.context.SaveChangesAsync();
            }            
        }

        private List<Q88ListingServiceModel> GetVesselListing(char fromId, char toId)
        {
            var all = new List<Q88ListingServiceModel>();

            for (char i = fromId; i < toId; i++)
            {
                try
                {
                    var vesselData = this.ScrapeVessel(i);
                    all.Add(vesselData);
                }
                catch { }
            }

            return all;
        }

        private int GetOrCreateType(string typeName)
        {
            var type = this.context
                .Types
                .FirstOrDefault(x => x.Name == typeName);

            if (type != null)
            {
                return type.Id;
            }

            type = new Data.Models.Type
            {
                Name = typeName
            };

            this.context.Types.Add(type);
            this.context.SaveChanges();

            return type.Id;
        }

        private int GetOrCreateOwner(string ownerName)
        {
            var owner = this.context
                .Owners
                .FirstOrDefault(x => x.Name == ownerName);

            if (owner != null)
            {
                return owner.Id;
            }

            owner = new Owner 
            {
                Name = ownerName
            };

            this.context.Owners.Add(owner);
            this.context.SaveChanges();

            return owner.Id;
        }

        private int GetOrCreateClassSociety(string classSocietyFullName)
        {
            var classSociety = this.context
                .ClassificationSocieties
                .FirstOrDefault(x => x.FullName == classSocietyFullName);

            if (classSociety != null)
            {
                return classSociety.Id;
            }

            classSociety = new ClassificationSociety 
            {
                FullName = classSocietyFullName
            };

            this.context.ClassificationSocieties.Add(classSociety);
            this.context.SaveChanges();

            return classSociety.Id;
        }

        private int GetOrCreateRegistration(string flagName, string registryPortName)
        {
            var registration = this.context
                .Registrations
                .FirstOrDefault(x => x.Flag == flagName);

            if (registration != null)
            {
                return registration.Id;
            }

            registration = new Registration 
            {
                Flag = flagName,
                RegistryPort = registryPortName
            };

            this.context.Registrations.Add(registration);
            this.context.SaveChanges();

            return registration.Id;
        }

        private Q88ListingServiceModel ScrapeVessel(char id)
        {
            var formattedUrl = string.Format(BASE_URL_BY_CHARACTER, id);
            var document = this.browsingContext
                .OpenAsync(formattedUrl)
                .GetAwaiter()
                .GetResult();            

            if (document.DocumentElement.OuterHtml.Contains(ERROR_MESSAGE))
            {
                throw new InvalidOperationException();
            }

            var vesselsData = new Q88ListingServiceModel();

            // Get Vessel Name
            var names = this.SelectorType(document, VESSEL_NAME_SELECTOR, 27);
            vesselsData.Name.AddRange(names);

            // Get IMOs 
            var imos = this.SelectorType(document, IMO_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Imo.AddRange(imos);

            // Get Built Data
            var builtData = this.SelectorType(document, BUILT_DATA_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Built.AddRange(builtData);

            // Get DTWs
            var dwts = this.SelectorType(document, DWT_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.SummerDwt.AddRange(dwts);

            // Get LOAs
            var loas = this.SelectorType(document, LOA_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Loa.AddRange(loas);

            // Get Cubic
            var cubic = this.SelectorType(document, CUBIC_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Cubic.AddRange(cubic);

            // Get Beams
            var beams = this.SelectorType(document, BEAM_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Beam.AddRange(beams);

            // Get Drafts
            var drafts = this.SelectorType(document, DRAFT_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Draft.AddRange(drafts);

            // Get Hulls
            var hullTypes = this.SelectorType(document, HULL_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.HullType.AddRange(hullTypes);

            //Get Call Sings
            var callSings = this.SelectorType(document, CALL_SIGN_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.CallSign.AddRange(callSings);

            return vesselsData;
        }

        private List<string> SelectorType(IDocument document, string selector, int skipNumber) 
            => document
                .QuerySelectorAll(selector)
                .Select(x => x.TextContent)
                .Skip(skipNumber)
                .ToList();

        // TODO: Try optimizing ScrapeOwners and ScrapeGuids if possible. Change variable names. Document the code for a better understanding later on
        private async Task<List<string>> ScrapeOwners(IBrowsingContext context, List<string> guids)
        {
            var owners = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                string formatted = this.UrlFormatting(guids, i);
                var document = await context.OpenAsync(formatted);

                var outerHtmlPerVessel = document
                    .QuerySelectorAll(OWNER_SELECTOR)
                    .Select(x => x.TextContent)
                    .ToList();

                owners.Add(outerHtmlPerVessel[0].Trim());
            }

            return owners;
        }

        private async Task<List<string>> ScrapeTypes(IBrowsingContext context, List<string> guids)
        {
            var types = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                string formatted = this.UrlFormatting(guids, i);
                var document = await context.OpenAsync(formatted);

                var outerHtmlPerVessel = document
                    .QuerySelectorAll(TYPE_SELECTOR)
                    .Select(x => x.TextContent)
                    .ToList();

                types.Add(outerHtmlPerVessel[0].Trim());
            }

            return types;
        }      

        private async Task<List<string>> ScrapeGuids(IBrowsingContext context, char id)
        {
            var formattedUrl = string.Format(BASE_URL_BY_ID, id);
            var document = await context.OpenAsync(formattedUrl);

            if (document.DocumentElement.OuterHtml.Contains(ERROR_MESSAGE))
            {
                System.Console.WriteLine($"{id} not found");
            }

            var outerHtmlPerVessel = document
                .QuerySelectorAll(GUID_SELECTOR)
                .Select(x => x.OuterHtml)
                .Skip(2)
                .ToList();

            var vesselGuids = new List<string>();

            foreach (var outerHtml in outerHtmlPerVessel)
            {
                var newOuterHtml = outerHtml
                    .Split("?id=", StringSplitOptions.RemoveEmptyEntries);
                var vesselGuid = newOuterHtml[1]
                    .Split("&amp", StringSplitOptions.RemoveEmptyEntries)[0];
                vesselGuids.Add(vesselGuid);
            }

            return vesselGuids;
        }

        private async Task<Dictionary<int, Dictionary<string, string>>> ScrapeRegistrationsWithPorts(IBrowsingContext context, List<string> guids)
        {
            var registrations = new Dictionary<int, Dictionary<string, string>>();

            for (int i = 0; i < guids.Count; i++)
            {
                string formatted = this.UrlFormatting(guids, i);
                var document = await context.OpenAsync(formatted);

                var outerHtmlPerVesselFlag = document
                    .QuerySelectorAll(FLAG_SELECTOR)
                    .Select(x => x.TextContent)
                    .ToList();

                var outerHtmlPerVesselPort = document
                    .QuerySelectorAll(PORT_SELECTOR)
                    .Select(x => x.TextContent)
                    .ToList();

                if (!registrations.ContainsKey(i))
                {
                    registrations.Add(i, new Dictionary<string, string>());
                }

                registrations[i].Add(outerHtmlPerVesselFlag[0], outerHtmlPerVesselPort[0]);
            }

            return registrations;
        }

        private async Task<List<string>> ScrapeClassSocieties(IBrowsingContext context, List<string> guids)
        {
            var classSocieties = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                string formatted = this.UrlFormatting(guids, i);
                var document = await context.OpenAsync(formatted);

                var outerHtmlPerVessel = document
                    .QuerySelectorAll(CLASS_SOCIETY_SELECTOR)
                    .Select(x => x.TextContent)
                    .ToList();

                classSocieties.Add(outerHtmlPerVessel[0].Trim());
            }

            return classSocieties;
        }

        private string UrlFormatting(List<string> guids, int i)
        {
            string url = "https://www.q88.com/ViewShip.aspx?id={0}";
            var formatted = String.Format(url, guids[i]);

            return formatted;
        }
    }
}
