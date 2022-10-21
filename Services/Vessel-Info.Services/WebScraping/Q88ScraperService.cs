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
    using Vessel_Info.Services.Models.WebScraping;
    using Vessel_Info.Services.Vessels;

    using static Constants.ServicesConstants;

    public class Q88ScraperService : IQ88ScraperService
    {      
        private readonly IConfiguration config;
        private readonly IBrowsingContext browsingContext;
        private readonly VesselInfoDbContext dbContext;

        private readonly ITypeService types;
        private readonly IOwnerService owners;
        private readonly IRegistrationService registrations;
        private readonly IClassificationSocietyService classificationSocieties;
        private readonly IOperatorService operators;
        private readonly IShipbrokerService shipbrokers;

        public Q88ScraperService(
            VesselInfoDbContext dbContext,
            ITypeService types,
            IOwnerService owners,
            IRegistrationService registrations,
            IClassificationSocietyService classificationSocieties,
            IOperatorService operators,
            IShipbrokerService shipbrokers)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(this.config);
            this.dbContext = dbContext;

            this.types = types;
            this.owners = owners;
            this.registrations = registrations;
            this.classificationSocieties = classificationSocieties;
            this.operators = operators;
            this.shipbrokers = shipbrokers;
        }

        public async Task ImportVesselDataAsync(char fromId = StartLetter, char toId = EndLetter)
        {
            var vessels = this.GetVesselListing(fromId, toId);
            var currenFromId = fromId;

            foreach (var vessel in vessels)
            {
                var names = vessel.Name;
                var summerDwts = vessel.SummerDwt;
                var loas = vessel.Loa;
                var imos = vessel.Imo;
                var hullTypes = vessel.HullType;
                var cubic = vessel.Cubic;
                var beams = vessel.Beam;
                var drafts = vessel.Draft;
                var callSigns = vessel.CallSign;
                var builts = vessel.Built;

                var guids = this.ScrapeGuids(this.browsingContext, currenFromId++);
                var owners = this.ScrapeOwners(this.browsingContext, guids);
                var types = this.ScrapeTypes(this.browsingContext, guids);
                var classSocieties = this.ScrapeClassSocieties(this.browsingContext, guids);
                var registrations = this.ScrapeRegistrationsWithPorts(this.browsingContext, guids);
                var operators = this.ScrapeOperators(this.browsingContext, guids); 

                // Size of each entity. They should always be equal.
                var vesselSize = names.Count;

                for (int i = 0; i < vesselSize; i++)
                {
                    var typeId = await this.types.GetOrCreateTypeAsync(types[i]);
                    var ownerId = await this.owners.GetOrCreateOwnerAsync(owners[i]);
                    var classSocietyId = await this.classificationSocieties.GetOrCreateClassSocietyAsync(classSocieties[i]);
                    var operatorId = await this.operators.GetOrCreateOperatorAsync(operators[i]);
                    // Shipbrokers data won't be scraped at this moment.
                    var shipbrokerId = await this.shipbrokers.GetOrCreateShipbrokerAsync("shipbroker");

                    var registrationKeys = registrations[i].Keys.FirstOrDefault();
                    var registrationValues = registrations[i].Values.FirstOrDefault();
                    var registrationId = await this.registrations.GetOrCreateRegistrationAsync(registrationKeys, registrationValues);

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
                        ClassificationSocietyId = classSocietyId,
                        ShipbrokerId = shipbrokerId
                    };

                    await this.dbContext.Vessels.AddAsync(newVessel);
                }

                await this.dbContext.SaveChangesAsync();
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

        private Q88ListingServiceModel ScrapeVessel(char id)
        {
            var formattedUrl = string.Format(BaseUrl, id);
            var document = this.GetDocument(formattedUrl);

            var vesselsData = new Q88ListingServiceModel();

            // Due to data inconsistency all data retrieved is stored in List<string> properties for easier persisting into the database
            // Get Vessel Name
            var names = this.SelectorType(document, VesselNameSelector, 27);
            vesselsData.Name.AddRange(names);

            // Get IMOs 
            var imos = this.SelectorType(document, ImoSelector, ScrapeVesselSkipNumber);
            vesselsData.Imo.AddRange(imos);

            // Get Built Data
            var builtData = this.SelectorType(document, BuiltDataSelector, ScrapeVesselSkipNumber);
            vesselsData.Built.AddRange(builtData);

            // Get DTWs
            var dwts = this.SelectorType(document, DwtSelector, ScrapeVesselSkipNumber);
            vesselsData.SummerDwt.AddRange(dwts);

            // Get LOAs
            var loas = this.SelectorType(document, LoaSelector, ScrapeVesselSkipNumber);
            vesselsData.Loa.AddRange(loas);

            // Get Cubic
            var cubic = this.SelectorType(document, CubicSelector, ScrapeVesselSkipNumber);
            vesselsData.Cubic.AddRange(cubic);

            // Get Beams
            var beams = this.SelectorType(document, BeamSelector, ScrapeVesselSkipNumber);
            vesselsData.Beam.AddRange(beams);

            // Get Drafts
            var drafts = this.SelectorType(document, DraftSelector, ScrapeVesselSkipNumber);
            vesselsData.Draft.AddRange(drafts);

            // Get Hulls
            var hullTypes = this.SelectorType(document, HullSelector, ScrapeVesselSkipNumber);
            vesselsData.HullType.AddRange(hullTypes);

            //Get Call Sings
            var callSings = this.SelectorType(document, CallSignSelector, ScrapeVesselSkipNumber);
            vesselsData.CallSign.AddRange(callSings);

            return vesselsData;
        }

        private List<string> ScrapeOwners(IBrowsingContext context, List<string> guids)
        {
            var owners = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(ViewShipUrl, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, OwnerSelector, ScrapeSkipNumber);

                owners.Add(outerHtmlPerVessel[0].Trim());
            }

            return owners;
        }

        private List<string> ScrapeTypes(IBrowsingContext context, List<string> guids)
        {
            var types = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(ViewShipUrl, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, TypeSelector, ScrapeSkipNumber);

                types.Add(outerHtmlPerVessel[0].Trim());
            }

            return types;
        }      

        private List<string> ScrapeGuids(IBrowsingContext context, char id)
        {
            var formattedUrl = string.Format(BaseUrl, id);
            var document = this.GetDocument(formattedUrl);

            var outerHtmlPerVessel = document
                .QuerySelectorAll(GuidSelector)
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

        private Dictionary<int, Dictionary<string, string>> ScrapeRegistrationsWithPorts(IBrowsingContext context, List<string> guids)
        {
            var registrations = new Dictionary<int, Dictionary<string, string>>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(ViewShipUrl, guids, i);
                var document = this.GetDocument(formattedUrl);

                var outerHtmlPerVesselFlag = this.SelectorType(document, FlagSelector, ScrapeSkipNumber);
                var outerHtmlPerVesselPort = this.SelectorType(document, PortSelector, ScrapeSkipNumber);

                if (!registrations.ContainsKey(i))
                {
                    registrations.Add(i, new Dictionary<string, string>());
                }

                registrations[i].Add(outerHtmlPerVesselFlag[0], outerHtmlPerVesselPort[0]);
            }

            return registrations;
        }

        private List<string> ScrapeClassSocieties(IBrowsingContext context, List<string> guids)
        {
            var classSocieties = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(ViewShipUrl, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, ClassSocietySelector, ScrapeSkipNumber);

                classSocieties.Add(outerHtmlPerVessel[0].Trim());
            }

            return classSocieties;
        }

        private List<string> ScrapeOperators(IBrowsingContext context, List<string> guids)
        {
            var operators = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(ViewShipUrl, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, OperatorSelector, ScrapeSkipNumber);

                operators.Add(outerHtmlPerVessel[0].Trim());
            }

            return operators;
        }

        private IDocument GetDocument(string formattedUrl)
        {
            var document = this.browsingContext
                .OpenAsync(formattedUrl)
                .GetAwaiter()
                .GetResult();

            if (document.DocumentElement.OuterHtml.Contains(ErrorMessage))
            {
                throw new InvalidOperationException();
            }

            return document;
        }

        private List<string> SelectorType(IDocument document, string selector, int skipNumber) 
            => document
                .QuerySelectorAll(selector)
                .Select(x => x.TextContent)
                .Skip(skipNumber)
                .ToList();

        private string UrlFormatting(string url, List<string> guids, int i)
        {
            var formatted = String.Format(url, guids[i]);

            return formatted;
        }
    }
}
