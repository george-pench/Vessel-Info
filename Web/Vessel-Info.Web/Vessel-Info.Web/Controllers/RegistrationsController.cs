namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Registrations;

    public class RegistrationsController : Controller
    {
        private readonly IRegistrationService registrations;

        public RegistrationsController(IRegistrationService registrations)
        {
            this.registrations = registrations;
        }

        public IActionResult All()
        {
            var all = this.registrations
                .All()
                .To<RegistrationBaseViewModel>();

            return this.View(all);
        }

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
