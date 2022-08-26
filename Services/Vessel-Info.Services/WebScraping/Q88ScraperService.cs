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

    public class Q88ScraperService : IQ88ScraperService
    {
        // TODO: move constants into a separate class
        private const string ERROR_MESSAGE = "No vessels starting with {0} were found.";
        private const string BASE_URL = "https://www.q88.com/ships.aspx?letter={0}&v=list";

        private const string VESSELNAME_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td > a";
        private const string IMO_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(2)";
        private const string BUILTDATA_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(3)";
        private const string DWT_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(4)";
        private const string LOA_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)";
        private const string CUBIC_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)";
        private const string BEAM_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)";
        private const string DRAFT_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)";
        private const string HULL_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(9)";
        private const string CALLSIGN_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(10)";
        private const int SKIPNUMBER_SELECTOR = 1;

        private readonly IConfiguration config;
        private readonly IBrowsingContext browsingContext;

        private readonly VesselInfoDbContext context;

        public Q88ScraperService(VesselInfoDbContext context)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(this.config);

            this.context = context;
        }

        public async Task ImportVesselDataAsync(char fromId = 'X', char toId = 'Y')
        {         
            var vessels = this.GetVesselListing(fromId, toId);
            Console.WriteLine();

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
            var vesselSize = imos.Count;

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

        private int GetOrCreateRegistration(string flagName, string registryPortName) // ToLower
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
            var formattedUrl = string.Format(BASE_URL, id);

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
            var names = this.SelectorType(document, VESSELNAME_SELECTOR, 27);
            vesselsData.Name.AddRange(names);

            // Get IMOs 
            var imos = this.SelectorType(document, IMO_SELECTOR, SKIPNUMBER_SELECTOR);
            vesselsData.Imo.AddRange(imos);

            // Get Built Data
            var builtData = this.SelectorType(document, BUILTDATA_SELECTOR, SKIPNUMBER_SELECTOR);
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
            var callSings = this.SelectorType(document, CALLSIGN_SELECTOR, SKIPNUMBER_SELECTOR);
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
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(6) > td:nth-child(5)")
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
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(5) > td:nth-child(2)")
                    .Select(x => x.TextContent)
                    .ToList();

                types.Add(outerHtmlPerVessel[0].Trim());
            }

            return types;
        }      

        private async Task<List<string>> ScrapeGuids(IBrowsingContext context, char id)
        {
            var document = await context.OpenAsync($"https://www.q88.com/ships.aspx?letter={id}&v=list");

            if (document.DocumentElement.OuterHtml.Contains("No vessels starting with 'Adsadas' were found."))
            {
                System.Console.WriteLine($"{id} not found");
            }

            // "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(1)"
            var outerHtmlPerVessel = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(1)")
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
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(3) > td:nth-child(2)")
                    .Select(x => x.TextContent)
                    .ToList();

                var outerHtmlPerVesselPort = document
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(4) > td:nth-child(2)")
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
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(7) > td:nth-child(2)")
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
