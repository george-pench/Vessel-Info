namespace Vessel_Info.Services.Vessels
{
    using System.Collections.Generic;
    using Vessel_Info.Services.Vessels.Models;

    public interface IVesselService
    {
        IEnumerable<VesselAllServiceModel> All();

        VesselDetailsServiceModel Details(string id);
    }
}
