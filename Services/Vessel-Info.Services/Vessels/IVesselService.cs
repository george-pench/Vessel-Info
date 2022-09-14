namespace Vessel_Info.Services.Vessels
{
    using System.Linq;
    using Vessel_Info.Services.Models.Vessels;

    public interface IVesselService
    {
        bool Create(VesselCreateServiceModel model);

        bool Delete(string id);

        bool Edit(string id, VesselEditServiceModel model);

        IQueryable<VesselAllServiceModel> All();

        VesselDetailsServiceModel Details(string id);

        VesselAllServiceModel GetById(string id);
    }
}
