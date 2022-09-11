namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselAllViewModel : VesselBaseViewModel, 
        IMapFrom<VesselAllServiceModel>, IMapTo<VesselAllServiceModel>,
        IMapFrom<VesselCreateInputModel>, IMapTo<VesselCreateInputModel>
    {
        public string Id { get; set; }       

        public string Loa { get; set; }

        public string Cubic { get; set; }

        public string Beam { get; set; }

        public string Draft { get; set; }
    }
}
