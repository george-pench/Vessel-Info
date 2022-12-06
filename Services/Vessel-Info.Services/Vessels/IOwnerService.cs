namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Owners;

    public interface IOwnerService
    {
        Task<OwnerAllServiceModel> GetByIdAsync(int? id);

        IQueryable<OwnerAllServiceModel> GetAllBySearchTerm(string searchTerm);

        Task<int> GetOrCreateOwnerAsync(string ownerName);

        Task<OwnerDetailsServiceModel> DetailsAsync(int? id);

        Task<bool> EditAsync(int? id, OwnerEditServiceModel model);

        Task<int> FindOwnerIdByNameAsync(string vesselOwner);

        IQueryable<OwnerBaseServiceModel> AllPaging(int page, int itemsPerPage = 12);

        IQueryable<OwnerBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
