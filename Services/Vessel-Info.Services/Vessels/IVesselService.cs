namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IVesselService
    {
        Task<string> CreateAsync(VesselCreateServiceModel model);
        
        Task<bool> EditAsync(string id, VesselEditServiceModel model);
        
        Task<VesselDetailsServiceModel> DetailsAsync(string id);

        Task DeleteAsync(string id);

        IQueryable<VesselAllServiceModel> All();

        Task<VesselAllServiceModel> GetByIdAsync(string id);
    }
}
