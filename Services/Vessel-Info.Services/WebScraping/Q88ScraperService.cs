namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using System;
    using System.Collections.Concurrent;
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
            var vessels = this.ScrapeRecipes(fromId, toId);
            Console.WriteLine();

            var vesselNames = vessels[0].VesselName;
            var summerDwts = vessels[0].SummerDwt;           
            var loas = vessels[0].LOA;
            var imos = vessels[0].IMO;
            var hullTypes = vessels[0].HullType;
            var cubic = vessels[0].Cubic;
            var callSigns = vessels[0].CallSign;
            var builts = vessels[0].Built;

            // Size of each entity. They should always be equal. 
            var vesselSize = imos.Count;

            for (int i = 0; i < vesselSize; i++)
            {
                var newVessel = new Vessel
                {
                    Name = vesselNames[i],
                    SummerDwt = summerDwts[i],
                    IMO = imos[i],
                    HullType = hullTypes[i],
                    CallSign = callSigns[i],
                    Built = DateTime.Parse(builts[i])
                };

                await this.context.Vessels.AddAsync(newVessel);

                Console.WriteLine($"{vesselNames[i]} {imos[i]} created");
            }


            //await this.context.SaveChangesAsync();
        }

        private List<Q88ListingServiceModel> ScrapeRecipes(char fromId, char toId)
        {
            var all = new List<Q88ListingServiceModel>();

            for (char i = fromId; i < toId; i++)
            {
                try
                {
                    var vesselData = this.GetVesselsData(i);
                    all.Add(vesselData);
                }
                catch { }
            }

            return all;
        }

        private int GetOrCreateVessel(string vesselImo)
        {
            var vessel = this.context
                .Vessels
                .FirstOrDefault(x => x.IMO == vesselImo);

            if (vessel != null)
            {
                return vessel.Id;
            }

            vessel = new Data.Models.Vessel
            {
                IMO = vesselImo,
            };

            this.context.Vessels.Add(vessel);
            this.context.SaveChanges();

            return vessel.Id;
        }

        private Q88ListingServiceModel GetVesselsData(char id)
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
                .Select(x => x.TextContent.Replace(",", string.Empty))
                .Skip(1)
                .Select(x => int.Parse(x))
                .ToList();

            vesselsData.SummerDwt.AddRange(DWTs);

            // Get LOAs
            var LOAs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)")
                .Select(x => x.TextContent.Replace(".", string.Empty))
                .Skip(1)
                .Select(x => int.Parse(x))
                .ToList();

            vesselsData.LOA.AddRange(LOAs);

            // Get Cubic
            var cubic = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)")
                .Select(x => x.TextContent.Replace(",", string.Empty))
                .Skip(1)
                .Select(x => int.Parse(x))
                .ToList();

            vesselsData.Cubic.AddRange(cubic);

            // Get Beams
            //var beams = document
            //    .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)")
            //    .Select(x => x.TextContent)
            //    .ToList();

            // Get Drafts
            //var drafts = document
            //    .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)")
            //    .Select(x => x.TextContent)
            //    .ToList();

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
    }
}
