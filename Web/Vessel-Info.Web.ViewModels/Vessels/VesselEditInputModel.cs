namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselEditInputModel : VesselDetailsViewModel, 
        IMapFrom<VesselEditServiceModel>, IMapTo<VesselEditServiceModel>,
        IMapFrom<VesselAllServiceModel>, IMapTo<VesselAllServiceModel>
    {
        public string Loa { get; set; }

        public string Cubic { get; set; }

        public string Beam { get; set; }

        public string Draft { get; set; }
    }
}
