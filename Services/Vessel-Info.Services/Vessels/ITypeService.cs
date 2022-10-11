namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface ITypeService
    {
        Task<int> GetOrCreateTypeAsync(string typeName);

        Task<VesselTypeServiceModel> DetailsAsync(int? id);

        Task<int> FindTypeIdByName(string vesselType);

        IQueryable<VesselTypeServiceModel> All();

        Task<int> GetCountAsync();
    }
}
