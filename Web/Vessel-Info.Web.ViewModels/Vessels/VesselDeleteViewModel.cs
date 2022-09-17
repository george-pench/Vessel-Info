namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselDeleteViewModel : VesselAllViewModel, 
        IMapFrom<VesselAllServiceModel>, IMapTo<VesselAllServiceModel> {}
}
