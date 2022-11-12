namespace Vessel_Info.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Vessels;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class VesselsController : AdminController
    {      
        // TODO: exception handling, data validation
        private readonly IVesselService vessels;
        private readonly IRegistrationService registrations;
        private readonly ITypeService types;
        private readonly IOwnerService owners;
        private readonly IClassificationSocietyService classificationSocieties;

        public VesselsController(
            IVesselService vessels, 
            IRegistrationService registrations,
            ITypeService types, 
            IOwnerService owners, 
            IClassificationSocietyService classificationSocieties)
        {
            this.vessels = vessels;
            this.registrations = registrations;
            this.types = types;
            this.owners = owners;
            this.classificationSocieties = classificationSocieties;
        }

        public IActionResult Search(string searchTerm) => this.View(new VesselListingViewModel
        {
            Vessels = this.vessels.GetAllBySearchTerm(searchTerm).To<VesselAllViewModel>()
        });

        // [HttpGet("Admin/Vessels/All")]
        public async Task<IActionResult> All(int id = 1)
        {           
            if (id < 0)
            {
                return this.NotFound();
            }
          
            return this.View(new VesselListingViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.vessels.GetCountAsync(),
                Vessels = this.vessels.AllPaging(id, ItemsPerPage).To<VesselAllViewModel>()
            });
        }

        // [HttpGet("Admin/Vessels/Details")]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }

            var details = (await this.vessels
                .DetailsAsync(id))
                .To<VesselDetailsViewModel>();

            return this.View(details);
        }

        // [HttpGet("Admin/Vessels/Create")]
        public IActionResult Create() => this.View(new VesselCreateInputModel
        {
            Registrations = this.registrations.All(),
            Types = this.types.All(),
            Owners = this.owners.All(),
            ClassificationSocieties = this.classificationSocieties.All(),

        });

        [HttpPost]
        public async Task<IActionResult> Create(VesselCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var create = ObjectMappingExtensions.To<VesselFormServiceModel>(model);
            create.Vessel.Id = Guid.NewGuid().ToString();            

            var vesselId = await this.vessels.CreateAsync(create);

            TempData[GlobalMessage] = $"{create.Vessel.Name} successfully added!";

            return this.RedirectToAction(nameof(VesselsController.Details), new { id = vesselId });
        }

        // [HttpGet("Admin/Vessels/Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }

            var edit = (await this.vessels
                .GetByIdAsync(id))
                .To<VesselEditInputModel>();

            edit.Registrations = this.registrations.All();
            edit.Types = this.types.All();
            edit.Owners = this.owners.All();
            edit.ClassificationSocieties = this.classificationSocieties.All();

            return this.View(edit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, VesselEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var edit = ObjectMappingExtensions.To<VesselFormServiceModel>(model);

            await this.vessels.EditAsync(id, edit);

            TempData[GlobalMessage] = $"{edit.Vessel.Name} successfully edited!";

            return this.RedirectToAction(nameof(VesselsController.Details), new { id = id });
        }

        // [HttpGet("Admin/Vessels/Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
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
            var deletedVesselName = (await this.vessels.GetByIdAsync(id)).Name;

            await this.vessels.DeleteAsync(id);

            TempData[GlobalMessage] = $"{deletedVesselName} successfully deleted!";

            return this.RedirectToAction(nameof(VesselsController.All));
        }
    }
}
