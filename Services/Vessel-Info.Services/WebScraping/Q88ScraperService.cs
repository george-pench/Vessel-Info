namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Models;

    public class Q88ScraperService : IQ88ScraperService
    {
        private const string ERROR_MESSAGE = "No vessels starting with {0} were found.";
        private const string BASE_URL = "https://www.q88.com/ships.aspx?letter={0}&v=list";

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

            var vesselNames = vessels[0].VesselName;
            var summerDwts = vessels[0].SummerDwt;           
            var loas = vessels[0].LOA;
            var imos = vessels[0].IMO;
            var hullTypes = vessels[0].HullType;
            var cubic = vessels[0].Cubic;
            var beams = vessels[0].Beam;
            var drafts = vessels[0].Draft;
            var callSigns = vessels[0].CallSign;
            var builts = vessels[0].Built;

            var guids = await this.ScrapeGuids(this.browsingContext, fromId);
            var owners = await this.ScrapeOwners(this.browsingContext, guids);
            var types = await this.ScrapeTypes(this.browsingContext, guids);
            var registrations = await this.ScrapeRegistrations(this.browsingContext, guids);

            // Size of each entity. They should always be equal.
            var vesselSize = imos.Count;

            for (int i = 0; i < vesselSize; i++)
            {
                var typeId = this.GetOrCreateType(types[i]);
                var ownerId = this.GetOrCreateOwner(owners[i]);
                var registrationId = this.GetOrCreateRegistration(registrations[i]);

                var newVessel = new Vessel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = vesselNames[i],
                    IMO = imos[i],
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

        private int GetOrCreateRegistration(string flagName)
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
                Flag = flagName
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
            var names = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td > a")
                .Select(x => x.TextContent)
                .Skip(27)
                .ToList();

            vesselsData.VesselName.AddRange(names);

            // Get IMOs
            var IMOs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(2)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.IMO.AddRange(IMOs);

            // Get Built Data
            var builtData = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(3)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.Built.AddRange(builtData);

            // Get DTWs
            var DWTs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(4)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.SummerDwt.AddRange(DWTs);

            // Get LOAs
            var LOAs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.LOA.AddRange(LOAs);

            // Get Cubic
            var cubic = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)")
                .Select(x => x.TextContent)                
                .Skip(1)
                .ToList();

            vesselsData.Cubic.AddRange(cubic);

            // Get Beams
            var beams = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.Beam.AddRange(beams);

            // Get Drafts
            var drafts = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.Draft.AddRange(drafts);

            // Get Hulls
            var hullTypes = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(9)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.HullType.AddRange(hullTypes);

            //Get Call Sings
            var callSings = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(10)")
                .Select(x => x.TextContent)
                .Skip(1)
                .ToList();

            vesselsData.CallSign.AddRange(callSings);

            return vesselsData;
        }

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

        private async Task<List<string>> ScrapeRegistrations(IBrowsingContext context, List<string> guids)
        {
            var registrations = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                string formatted = this.UrlFormatting(guids, i);
                var document = await context.OpenAsync(formatted);

                var outerHtmlPerVessel = document
                    .QuerySelectorAll("#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(3) > td:nth-child(2)")
                    .Select(x => x.TextContent)
                    .ToList();

                registrations.Add(outerHtmlPerVessel[0].Trim());
            }

            return registrations;
        }

        private string UrlFormatting(List<string> guids, int i)
        {
            string url = "https://www.q88.com/ViewShip.aspx?id={0}";
            var formatted = String.Format(url, guids[i]);

            return formatted;
        }
    }
}
