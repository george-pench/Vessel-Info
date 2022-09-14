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
        // TODO: use async Task and revise functionality
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
                return this.View();
            }

            var create = ObjectMappingExtensions.To<VesselCreateServiceModel>(model);
            create.Vessel.Id = Guid.NewGuid().ToString();

            this.vessels.Create(create);

            return this.RedirectToAction(nameof(this.All));
        }

        // [HttpGet("Admin/Vessels/Edit")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var edit = this.vessels
                .GetById(id)
                .To<VesselEditInputModel>();

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.View(edit);
        }

        [HttpPost]
        public IActionResult Edit(string id, VesselEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var edit = ObjectMappingExtensions.To<VesselEditServiceModel>(model);
            this.vessels.Edit(id, edit);

            return this.RedirectToAction(nameof(this.All));
        }

        // [HttpGet("Admin/Vessels/Delete")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var delete = this.vessels
                .GetById(id)
                .To<VesselDeleteViewModel>();

            if (delete == null)
            {
                return this.NotFound();
            }

            return this.View(delete);
        }

        // [HttpGet("Admin/Vessels/Delete/id")]
        [HttpPost]
        public IActionResult DeleteConfirm(string id)
        {
            this.vessels.Delete(id);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
