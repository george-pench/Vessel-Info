namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Types;

    public interface ITypeService
    {
        IQueryable<TypeBaseServiceModel> GetAllBySearchTerm(string searchTerm);

        Task<int> GetOrCreateTypeAsync(string typeName);

        Task<TypeBaseServiceModel> DetailsAsync(int? id);

        Task<int> FindTypeIdByName(string vesselType);

        IQueryable<TypeBaseServiceModel> AllPaging(int page, int pageSize = 12);

        IQueryable<TypeBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
