namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IVesselService
    {
        Task<string> CreateAsync(VesselFormServiceModel model);

        Task<bool> EditAsync(string id, VesselFormServiceModel model);

        Task<VesselDetailsServiceModel> DetailsAsync(string id);

        Task DeleteAsync(string id);

        IQueryable<VesselAllServiceModel> All(int page, int itemsPerPage = 12);

        Task<VesselAllServiceModel> GetByIdAsync(string id);

        Task<int> GetCountAsync();

        IQueryable<VesselByTypeServiceModel> GetAllVesselByType();

        IQueryable<VesselByRegistrationServiceModel> GetAllVesselByRegistration();

        IQueryable<VesselByOwnerServiceModel> GetAllVesselByOwner();

        IQueryable<VesselByClassSocietyServiceModel> GetAllVesselByClassSociety();
    }
}
