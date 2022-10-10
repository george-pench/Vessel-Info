namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Types;

    public class TypesController : Controller
    {
        private readonly ITypeService types;

        public TypesController(ITypeService types)
        {
            this.types = types;
        }

        public IActionResult All()
        {
            var all = this.types
                .All()
                .To<TypeBaseViewModel>();

            return this.View(all);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var details = (await this.types
              .DetailsAsync(id))
              .To<TypeBaseViewModel>();

            return this.View(details);
        }
    }
}
