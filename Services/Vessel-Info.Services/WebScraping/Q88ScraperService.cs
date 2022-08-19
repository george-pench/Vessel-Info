namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Data;
    using Vessel_Info.Services.Models;

    public class Q88ScraperService : IQ88ScraperService
    {
        private static string ERROR_MESSAGE = "No vessels starting with 'Adsadas' were found.";

        private readonly IConfiguration config;
        private readonly IBrowsingContext browsingContext;

        private readonly VesselInfoDbContext context;

        public Q88ScraperService(VesselInfoDbContext context)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.browsingContext = BrowsingContext.New(this.config);

            this.context = context;
        }

        public async Task PopulateDatabase()
        {
            var concurrenetBag = new ConcurrentBag<Q88ServiceModel>();

            for (char i = 'A'; i < 'Z'; i++)
            {
                try
                {
                    var vesselData = this.GetVesselData(i);
                    concurrenetBag.Add(vesselData);
                } 
                catch { }
            }

            foreach (var vessel in concurrenetBag)
            {
                // var vesseTypeId = await this.GetOrCreateVesselTypeAsync(vessel.HullType);


            }

            // this.context.SaveChanges();
        }

        //private async Task<int> GetOrCreateVesselTypeAsync(string typeName)
        //{
        //    var type = this.context
        //        .Types
        //        // Get All Types
        //        .FirstOrDefault(t => t.Name == typeName);

        //    if (type == null)
        //    {
        //        type = new Data.Models.Type()
        //        {
        //            Name = typeName,
        //        };

        //        await this.context.Types.AddAsync(type);
        //    }

        //    return type.Id;
        //}

        public Q88ServiceModel GetVesselData(char id)
        {
            var document = this.browsingContext
                .OpenAsync($"https://www.q88.com/ships.aspx?letter={id}&v=list")
                .GetAwaiter()
                .GetResult();

            if (document.DocumentElement.OuterHtml.Contains(ERROR_MESSAGE))
            {
                throw new InvalidOperationException();
            }

            var vesselData = new Q88ServiceModel();

            // Get Vessel Name
            var names = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td > a")
                .Select(x => x.TextContent)
                .Skip(27);

            // Get IMOs
            var IMOs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(2)");

            // Get Built Data
            var builtData = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(3)");

            // Get DTWs
            var DWTs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(4)");

            // Get LOAs
            var LOAs = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)");

            // Get Cubic
            var cubic = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)");

            // Get Beam
            var beams = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)");

            // Get Draft
            var drafts = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)");

            // Get Hull
            var hulls = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(9)");

            //Get Call Sing
            var callSings = document
                .QuerySelectorAll("#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(10)");

            return vesselData;
        }
    }
}
