namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Owners;

    public interface IOwnerService
    {
        Task<OwnerAllServiceModel> GetById(int? id);

        Task<int> GetOrCreateOwnerAsync(string ownerName);

        Task<OwnerDetailsServiceModel> DetailsAsync(int? id);

        Task<bool> EditAsync(int? id, OwnerEditServiceModel model);

        Task<int> FindOwnerIdByName(string vesselOwner);

        IQueryable<OwnerBaseServiceModel> All(int page, int itemsPerPage = 12);

        Task<int> GetCountAsync();
    }
}
