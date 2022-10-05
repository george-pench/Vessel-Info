namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IRegistrationService
    {
        Task<int> GetOrCreateRegistrationAsync(string flagName, string registryPortName);

        Task<int> FindRegistrationIdByName(string vesselRegistration);

        IQueryable<VesselRegistrationServiceModel> All();

        Task<int> GetCountAsync();
    }
}
