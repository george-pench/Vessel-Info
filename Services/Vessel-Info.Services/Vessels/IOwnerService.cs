namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Owners;

    public interface IOwnerService
    {
        Task<int> GetOrCreateOwnerAsync(string ownerName);

        Task<OwnerBaseServiceModel> DetailsAsync(int? id);

        Task<int> FindOwnerIdByName(string vesselOwner);

        IQueryable<OwnerBaseServiceModel> All();

        Task<int> GetCountAsync();
    }
}
