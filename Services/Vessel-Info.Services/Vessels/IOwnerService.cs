namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using System.Threading.Tasks;
    using Vessel_Info.Services.Models.Vessels;

    public interface IOwnerService
    {
        Task<int> Create(VesselOwnerServiceModel model);

        Task<int> FindOwnerIdByName(string vesselOwner);

        IQueryable<VesselOwnerServiceModel> All();
    }
}
