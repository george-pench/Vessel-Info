namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.ClassSocieties;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class ClassSocietiesController : Controller
    {
        private readonly IClassificationSocietyService classificationSocieties;

        public ClassSocietiesController(IClassificationSocietyService classificationSocieties) 
            => this.classificationSocieties = classificationSocieties;

        public IActionResult Search(string searchTerm) => this.View(new ClassSocietyListingViewModel
        {
            ClassSocieties = this.classificationSocieties.GetAllBySearchTerm(searchTerm).To<ClassSocietyBaseViewModel>()
        });

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            return this.View(new ClassSocietyListingViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.classificationSocieties.GetCountAsync(),
                ClassSocieties = this.classificationSocieties.AllPaging(id, ItemsPerPage).To<ClassSocietyDetailsViewModel>()
            });
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.classificationSocieties
              .DetailsAsync(id))
              .To<ClassSocietyDetailsViewModel>();

            return this.View(details);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var edit = (await this.classificationSocieties
                .GetByIdAsync(id))
                .To<ClassSocietyEditInputModel>();

            return this.View(edit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ClassSocietyEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(model);
            }

            var edit = ObjectMappingExtensions.To<ClassSocietyEditServiceModel>(model);

            await this.classificationSocieties.EditAsync(id, edit);

            return this.RedirectToAction(nameof(ClassSocietiesController.Details), new { id = id });
        }
    }
}
