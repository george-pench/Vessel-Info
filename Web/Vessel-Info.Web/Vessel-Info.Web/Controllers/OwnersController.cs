namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Owners;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class OwnersController : Controller
    {
        private readonly IOwnerService owners;

        public OwnersController(IOwnerService owners)
        {
            this.owners = owners;
        }

        public IActionResult Search(string searchTerm) => this.View(new OwnerListingViewModel
        {
            Owners = this.owners.GetAllBySearchTerm(searchTerm).To<OwnerBaseViewModel>()
        });

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            return this.View(new OwnerListingViewModel 
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.owners.GetCountAsync(),
                Owners = this.owners.AllPaging(id, ItemsPerPage).To<OwnerBaseViewModel>()
            });
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.owners
              .DetailsAsync(id))
              .To<OwnerDetailsViewModel>();

            return this.View(details);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var edit = (await this.owners
                .GetById(id))
                .To<OwnerEditInputModel>();

            return this.View(edit);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, OwnerEditInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var edit = ObjectMappingExtensions.To<OwnerEditServiceModel>(model);

            await this.owners.EditAsync(id, edit);

            return this.RedirectToAction(nameof(OwnersController.Details), new { id = id });
        }
    }
}
