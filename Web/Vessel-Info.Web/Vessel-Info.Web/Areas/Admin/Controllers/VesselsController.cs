namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Vessels;

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
            var all = this.vessels
                .All()
                .To<VesselAllViewModel>();

            return this.View(all);
        }

        // [HttpGet("Admin/Vessels/Details")]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = this.vessels
                .Details(id)
                .To<VesselDetailsViewModel>();

            if (!this.ModelState.IsValid)
            {
                return this.View(details);
            }

            return this.View(details);
        }

        // [HttpGet("Admin/Vessels/Create")]
        public IActionResult Create() => this.View();

        [HttpPost]
        public IActionResult Create(VesselCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var vesselCreateServiceModel = ObjectMappingExtensions.To<VesselCreateServiceModel>(model);
            vesselCreateServiceModel.Vessel.Id = Guid.NewGuid().ToString();

            this.vessels.Create(vesselCreateServiceModel);

            return this.RedirectToAction(nameof(All));
        }
    }
}
