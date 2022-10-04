namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IClassificationSocietyService
    {
        Task<int> GetOrCreateClassSocietyAsync(string classSocietyFullName);

        Task<VesselClassificationSocietyServiceModel> DetailsAsync(int? id);

        Task<int> FindClassificationSocietyIdByName(string vesselClass);

        IQueryable<VesselClassificationSocietyServiceModel> All();
    }
}
