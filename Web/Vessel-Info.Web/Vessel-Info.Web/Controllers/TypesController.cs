namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
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

        public IActionResult All() => this.View(this.types
                .All()
                .To<TypeBaseViewModel>());
    }
}
