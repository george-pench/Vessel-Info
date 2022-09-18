namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IVesselService
    {
        Task<string> Create(VesselCreateServiceModel model);

        bool Delete(string id);

        bool Edit(string id, VesselEditServiceModel model);

        IQueryable<VesselAllServiceModel> All();

        Task<VesselDetailsServiceModel> Details(string id);

        Task<VesselAllServiceModel> GetById(string id);
    }
}
