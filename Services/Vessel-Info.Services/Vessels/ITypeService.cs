namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface ITypeService
    {
        Task<int> GetOrCreateTypeAsync(string typeName);

        Task<int> FindTypeIdByName(string vesselType);

        IQueryable<VesselTypeServiceModel> All();
    }
}
