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

        public VesselsController(IVesselService vessels) => this.vessels = vessels;

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
        public async Task<IActionResult> GetTypeMax()
        {
            var vesselTypeMaxCount = this.vessels
                .GetAllVesselByType()
                .Select(x => new
                {
                    TypeId = x.VesselType.Id,
                    TypeName = x.VesselType.Name
                })
                .AsEnumerable()
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();

            var result = await this.vessels
                .GetAllVesselByType()
                .Where(x => x.VesselType.Id == vesselTypeMaxCount.Key.TypeId)
                .To<VesselByTypeViewModel>()
                .ToListAsync();

            return this.View(result);
        }

        [Authorize]
        public async Task<IActionResult> GetRegistrationMax()
        {
            var vesselRegistrationMaxCount = this.vessels
                .GetAllVesselByRegistration()
                .Select(x => new 
                {
                    RegId = x.VesselRegistration.Id,
                    RedFlag = x.VesselRegistration.Flag,
                    RegPort = x.VesselRegistration.RegistryPort
                })
                .AsEnumerable()
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();

            var result = await this.vessels
                .GetAllVesselByRegistration()
                .Where(x => x.VesselRegistration.Id == vesselRegistrationMaxCount.Key.RegId)
                .To<VesselByRegistrationViewModel>()
                .ToListAsync();

            return this.View(result);
        }

        [Authorize]
        public async Task<IActionResult> GetOwnerMax()
        {
            var vesselOwnerMaxCount = this.vessels
                .GetAllVesselByOwner()
                .Select(x => new
                {
                    OwnerId = x.VesselOwner.Id,
                    OwnerName = x.VesselOwner.Name
                })
                .AsEnumerable()
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();

            var result = await this.vessels
                .GetAllVesselByOwner()
                .Where(x => x.VesselOwner.Id == vesselOwnerMaxCount.Key.OwnerId)
                .To<VesselByOwnerViewModel>()
                .ToListAsync();

            return this.View(result);
        }

        [Authorize]
        public async Task<IActionResult> GetClassSocietyMax()
        {
            var vesselClassSocietyMaxCount = this.vessels
                .GetAllVesselByClassSociety()
                .Select(x => new
                {
                    ClassSocietyId = x.VesselClassSociety.Id,
                    ClassSocietyName = x.VesselClassSociety.FullName
                })
                .AsEnumerable()
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .FirstOrDefault();

            var result = await this.vessels
                .GetAllVesselByClassSociety()
                .Where(x => x.VesselClassSociety.Id == vesselClassSocietyMaxCount.Key.ClassSocietyId)
                .To<VesselByClassSocietyViewModel>()
                .ToListAsync();

            return this.View(result);
        }
    }
}
