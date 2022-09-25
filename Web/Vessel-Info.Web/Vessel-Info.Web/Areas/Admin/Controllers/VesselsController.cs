namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Vessels;

    public class VesselsController : AdminController
    {
        // TODO: exception handling and functionality
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
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.vessels
                .DetailsAsync(id))
                .To<VesselDetailsViewModel>();

            return this.View(details);
        }

        // [HttpGet("Admin/Vessels/Create")]
        public IActionResult Create() => this.View();

        [HttpPost]
        public async Task<IActionResult> Create(VesselCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var create = ObjectMappingExtensions.To<VesselCreateServiceModel>(model);
            create.Vessel.Id = Guid.NewGuid().ToString();  

            var vesselId = await this.vessels.CreateAsync(create);

            return this.RedirectToAction(nameof(VesselsController.Details), new { id = vesselId });
        }

        // [HttpGet("Admin/Vessels/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var edit = (await this.vessels
                .GetByIdAsync(id))
                .To<VesselEditInputModel>();

            return this.View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, VesselEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var edit = ObjectMappingExtensions.To<VesselEditServiceModel>(model);
            edit.Vessel.Id = id;

            await this.vessels.EditAsync(id, edit);

            return this.RedirectToAction(nameof(VesselsController.All));
        }

        // [HttpGet("Admin/Vessels/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var delete = (await this.vessels
                .GetByIdAsync(id))
                .To<VesselDeleteViewModel>();

            return this.View(delete);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            await this.vessels.DeleteAsync(id);

            return this.RedirectToAction(nameof(VesselsController.All));
        }
    }
}
