namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Web.ViewModels.Types;
    
    public class VesselByTypeViewModel : VesselBaseViewModel, IMapFrom<VesselByTypeServiceModel>
    {
        public TypeBaseViewModel VesselType { get; set; }
    }
}
