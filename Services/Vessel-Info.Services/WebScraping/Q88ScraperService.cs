﻿namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using AngleSharp.Dom;
    using Microsoft.EntityFrameworkCore;
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
        private readonly VesselInfoDbContext dbContext;

        public Q88ScraperService(VesselInfoDbContext dbContext)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(this.config);
            this.dbContext = dbContext;
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

                var guids = this.ScrapeGuids(this.browsingContext, fromId);
                var owners = this.ScrapeOwners(this.browsingContext, guids);
                var types = this.ScrapeTypes(this.browsingContext, guids);
                var classSocieties = this.ScrapeClassSocieties(this.browsingContext, guids);
                var registrations = this.ScrapeRegistrationsWithPorts(this.browsingContext, guids);
                var operators = this.ScrapeOperators(this.browsingContext, guids);

                // Size of each entity. They should always be equal.
                var vesselSize = names.Count;

                for (int i = 0; i < vesselSize; i++)
                {
                    var typeId = await this.GetOrCreateTypeAsync(types[i]);
                    var ownerId = await this.GetOrCreateOwnerAsync(owners[i]);
                    var classSocietyId = await this.GetOrCreateClassSocietyAsync(classSocieties[i]);
                    // var operatorId = await this.GetOrCreateOperator(operators[i]);

                    var registrationKeys = registrations[i].Keys.FirstOrDefault();
                    var registrationValues = registrations[i].Values.FirstOrDefault();
                    var registrationId = await this.GetOrCreateRegistrationAsync(registrationKeys, registrationValues);

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

        private async Task<int> GetOrCreateTypeAsync(string typeName)
        {
            var type = await this.dbContext
                .Types
                .FirstOrDefaultAsync(x => x.Name == typeName);

            if (type != null)
            {
                return type.Id;
            }

            type = new Data.Models.Type
            {
                Name = typeName
            };

            await this.dbContext.Types.AddAsync(type);
            await this.dbContext.SaveChangesAsync();

            return type.Id;
        }

        private async Task<int> GetOrCreateOwnerAsync(string ownerName)
        {
            var owner = await this.dbContext
                .Owners
                .FirstOrDefaultAsync(x => x.Name == ownerName);

            if (owner != null)
            {
                return owner.Id;
            }

            owner = new Owner 
            {
                Name = ownerName
            };

            await this.dbContext.Owners.AddAsync(owner);
            await this.dbContext.SaveChangesAsync();

            return owner.Id;
        }

        private async Task<int> GetOrCreateClassSocietyAsync(string classSocietyFullName)
        {
            var classSociety = await this.dbContext
                .ClassificationSocieties
                .FirstOrDefaultAsync(x => x.FullName == classSocietyFullName);

            if (classSociety != null)
            {
                return classSociety.Id;
            }

            classSociety = new ClassificationSociety 
            {
                FullName = classSocietyFullName
            };

            await this.dbContext.ClassificationSocieties.AddAsync(classSociety);
            await this.dbContext.SaveChangesAsync();

            return classSociety.Id;
        }

        private async Task<int> GetOrCreateRegistrationAsync(string flagName, string registryPortName)
        {
            var registration = await this.dbContext
                .Registrations
                .FirstOrDefaultAsync(x => x.Flag == flagName);

            if (registration != null)
            {
                return registration.Id;
            }

            registration = new Registration 
            {
                Flag = flagName,
                RegistryPort = registryPortName
            };

            await this.dbContext.Registrations.AddAsync(registration);
            await this.dbContext.SaveChangesAsync();

            return registration.Id;
        }

        private async Task<int> GetOrCreateOperator(string operatorName)
        {
            var @operator = await this.dbContext
                .Operators
                .FirstOrDefaultAsync(x => x.Name == operatorName);

            if (@operator != null)
            {
                return @operator.Id;
            }

            @operator = new Operator 
            {
                Name = operatorName
            };

            await this.dbContext.Operators.AddAsync(@operator);
            await this.dbContext.SaveChangesAsync();

            return @operator.Id;
        }

        private Q88ListingServiceModel ScrapeVessel(char id)
        {
            var formattedUrl = string.Format(BASE_URL, id);
            var document = this.GetDocument(formattedUrl);

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

        private List<string> ScrapeOwners(IBrowsingContext context, List<string> guids)
        {
            var owners = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(VIEW_SHIP_URL, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, OWNER_SELECTOR, 0);

                owners.Add(outerHtmlPerVessel[0].Trim());
            }

            return owners;
        }

        private List<string> ScrapeTypes(IBrowsingContext context, List<string> guids)
        {
            var types = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(VIEW_SHIP_URL, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, TYPE_SELECTOR, 0);

                types.Add(outerHtmlPerVessel[0].Trim());
            }

            return types;
        }      

        private List<string> ScrapeGuids(IBrowsingContext context, char id)
        {
            var formattedUrl = string.Format(BASE_URL, id);
            var document = this.GetDocument(formattedUrl);

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

        private Dictionary<int, Dictionary<string, string>> ScrapeRegistrationsWithPorts(IBrowsingContext context, List<string> guids)
        {
            var registrations = new Dictionary<int, Dictionary<string, string>>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(VIEW_SHIP_URL, guids, i);
                var document = this.GetDocument(formattedUrl);

                var outerHtmlPerVesselFlag = this.SelectorType(document, FLAG_SELECTOR, 0);
                var outerHtmlPerVesselPort = this.SelectorType(document, PORT_SELECTOR, 0);

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
                var formattedUrl = this.UrlFormatting(VIEW_SHIP_URL, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, CLASS_SOCIETY_SELECTOR, 0);

                classSocieties.Add(outerHtmlPerVessel[0].Trim());
            }

            return classSocieties;
        }

        private List<string> ScrapeOperators(IBrowsingContext context, List<string> guids)
        {
            var operators = new List<string>();

            for (int i = 0; i < guids.Count; i++)
            {
                var formattedUrl = this.UrlFormatting(VIEW_SHIP_URL, guids, i);
                var document = this.GetDocument(formattedUrl);
                var outerHtmlPerVessel = this.SelectorType(document, OPERATOR_SELECTOR, 0);

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

            if (document.DocumentElement.OuterHtml.Contains(ERROR_MESSAGE))
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
