namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels;
    using Vessel_Info.Web.ViewModels.Home;

    using static Vessel_Info.Web.Constants.WebConstants;
    public class HomeController : Controller
    {
        private readonly IGetCountsService counts;

        public HomeController(IGetCountsService counts) => this.counts = counts;

        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole(Admin.AdministratorRoleName))
            {
                TempData[GlobalMessage] = $"Welcome Admin!";
            }

            return this.View((await this.counts.GetAllCountsAsync()).To<IndexViewModel>());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
