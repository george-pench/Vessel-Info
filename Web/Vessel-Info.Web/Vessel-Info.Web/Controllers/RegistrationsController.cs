namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Registrations;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class RegistrationsController : Controller
    {
        private readonly IRegistrationService registrations;

        public RegistrationsController(IRegistrationService registrations) => this.registrations = registrations;

        public IActionResult Search(string searchTerm) => this.View(new RegistrationListingViewModel
        {
            Registrations = this.registrations.GetAllBySearchTerm(searchTerm).To<RegistrationBaseViewModel>()
        });

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }
            
            return this.View(new RegistrationListingViewModel 
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.registrations.GetCountAsync(),
                Registrations = this.registrations.AllPaging(id, ItemsPerPage).To<RegistrationBaseViewModel>()
            });
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.registrations
              .DetailsAsync(id))
              .To<RegistrationBaseViewModel>();

            return this.View(details);
        }
    }
}
