namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.WebScraping;

    using static Vessel_Info.Services.Constants.ServicesConstants;
    using static Vessel_Info.Web.Constants.WebConstants;

    public class ScrapingController : AdminController
    {
        private readonly IQ88ScraperService scraperService;

        public ScrapingController(IQ88ScraperService scraperService) => this.scraperService = scraperService;

        public IActionResult Index() => this.View();

        public async Task<IActionResult> Scrape()
        {
            await this.scraperService.ImportVesselDataAsync(StartLetter, EndLetter);
            
            TempData[GlobalMessage] = $"Data have been scraped successfully!";
            
            return this.View();
        }
    }
}
