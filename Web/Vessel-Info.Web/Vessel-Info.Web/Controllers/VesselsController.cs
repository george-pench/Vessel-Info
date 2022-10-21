namespace Vessel_Info.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Vessels;
    using Vessel_Info.Web.ViewModels.Vessels;

    using static Vessel_Info.Web.Constants.WebConstants;

    public class VesselsController : Controller
    {
        private readonly IVesselService vessels;
        private readonly ITypeService types;

        public VesselsController(
            IVesselService vessels,
            ITypeService types)
        {
            this.vessels = vessels;
            this.types = types;
        }

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
                Vessels = this.vessels.All(id, ItemsPerPage).To<VesselAllViewModel>()
            });
        }

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> TypeMaxCount()
        {
            // TODO: sorting by different type of vessels
            var maxCount = await this.types.GetTypeMaxCountByFrequencyAsync();

            var result = await this.vessels
                .GetAllVesselByType()
                .Where(v => v.VesselType.Id == maxCount)
                .To<VesselByTypeViewModel>()
                .ToListAsync();

            return this.View(result);
        }
    }
}
