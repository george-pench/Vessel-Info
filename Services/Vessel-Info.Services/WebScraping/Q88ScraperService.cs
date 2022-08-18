namespace Vessel_Info.Services.WebScraping
{
    using AngleSharp;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Q88ScraperService : IQ88ScraperService
    {
        private static string ERROR_MESSAGE = "No vessels starting with 'Adsadas' were found.";

        private readonly IConfiguration config;
        private readonly IBrowsingContext context;

        public Q88ScraperService()
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
        }

        public async Task GetVesselData()
        {
            var document = await context.OpenAsync("https://www.q88.com/ships.aspx?letter=A&v=list");

            if (document.DocumentElement.OuterHtml.Contains(ERROR_MESSAGE))
            {
                return;
            }

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
        }
    }
}
