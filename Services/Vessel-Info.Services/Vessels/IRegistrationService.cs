namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Registrations;

    public interface IRegistrationService
    {
        Task<RegistrationBaseServiceModel> GetByIdAsync(int? id);

        IQueryable<RegistrationBaseServiceModel> GetAllBySearchTerm(string searchTerm);

        Task<int> GetOrCreateRegistrationAsync(string flagName, string registryPortName);

        Task<RegistrationBaseServiceModel> DetailsAsync(int? id);

        Task<bool> EditAsync(int? id, RegistrationBaseServiceModel model);

        Task<int> FindRegistrationIdByName(string vesselRegistration);

        IQueryable<RegistrationBaseServiceModel> AllPaging(int page, int pageSize = 12);

        IQueryable<RegistrationBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
