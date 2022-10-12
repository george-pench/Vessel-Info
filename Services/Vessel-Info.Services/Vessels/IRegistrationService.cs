namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Registrations;

    public interface IRegistrationService
    {
        Task<int> GetOrCreateRegistrationAsync(string flagName, string registryPortName);

        Task<RegistrationBaseServiceModel> DetailsAsync(int? id);

        Task<int> FindRegistrationIdByName(string vesselRegistration);

        IQueryable<RegistrationBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
