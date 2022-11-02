namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Types;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class TypesController : Controller
    {
        private readonly ITypeService types;

        public TypesController(ITypeService types)
        {
            this.types = types;
        }

        public IActionResult Search(string searchTerm) => this.View(new TypeListingViewModel
        {
            Types = this.types.GetAllBySearchTerm(searchTerm).To<TypeBaseViewModel>()
        });

        public async Task<IActionResult> All(int id = 1)
        {
            if (id < 0)
            {
                return this.NotFound();
            }

            return this.View(new TypeListingViewModel
            {
                ItemsPerPage = ItemsPerPage,
                PageNumber = id,
                EntityCount = await this.types.GetCountAsync(),
                Types = this.types.AllPaging(id, ItemsPerPage).To<TypeBaseViewModel>()
            });
        }
    }
}
