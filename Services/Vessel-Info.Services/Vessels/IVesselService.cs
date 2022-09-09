namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using Vessel_Info.Services.Models.Vessels;

    public interface IVesselService
    {
        bool Create(VesselCreateServiceModel model);

        IQueryable<VesselAllServiceModel> All();

        VesselDetailsServiceModel Details(string id);
    }
}
