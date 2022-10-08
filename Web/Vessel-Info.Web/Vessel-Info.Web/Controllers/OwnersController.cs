namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Owners;

    public class OwnersController : Controller
    {
        private readonly IOwnerService owners;

        public OwnersController(IOwnerService owners)
        {
            this.owners = owners;
        }

        public IActionResult All()
        {
            var all = this.owners
                .All()
                .To<OwnerBaseViewModel>();

            return this.View(all);
        }

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
    }
}
