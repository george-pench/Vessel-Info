namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IRegistrationService
    {
        Task<int> Create(VesselRegistrationServiceModel model);

        Task<int> FindRegistrationIdByName(string vesselRegistration);
    }
}
