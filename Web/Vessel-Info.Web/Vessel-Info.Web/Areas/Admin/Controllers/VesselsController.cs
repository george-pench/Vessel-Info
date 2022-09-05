namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Vessel_Info.Services.Vessels;

    public class VesselsController : AdminController
    {
        private readonly IVesselService vessels;

        public VesselsController(IVesselService vessels)
        {
            this.vessels = vessels;
        }

        // [HttpGet("Admin/Vessels/All")]
        public IActionResult All()
        {
            var vessels = this.vessels.All();

            return this.View(vessels);
        }

        // [HttpGet("Admin/Vessels/Details")]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var vessel = this.vessels.Details(id);

            if (vessel == null)
            {
                return this.NotFound();
            }

            return this.View(vessel);
        }
    }
}
