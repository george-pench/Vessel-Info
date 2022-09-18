namespace Vessel_Info.Services.Vessels
{
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IClassificationSocietyService
    {
        Task<int> Create(VesselClassificationSocietyServiceModel model);

        Task<int> FindClassificationSocietyIdByName(string vesselClass);
    }
}
