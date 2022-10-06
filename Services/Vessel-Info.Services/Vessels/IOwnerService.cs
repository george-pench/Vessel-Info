namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IOwnerService
    {
        Task<int> GetOrCreateOwnerAsync(string ownerName);

        Task<VesselOwnerServiceModel> DetailsAsync(int? id);

        Task<int> FindOwnerIdByName(string vesselOwner);

        IQueryable<VesselOwnerServiceModel> All();

        Task<int> GetCountAsync();
    }
}
