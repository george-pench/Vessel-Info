namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselDeleteViewModel : VesselBaseViewModel, 
        IMapFrom<VesselAllServiceModel>, IMapTo<VesselAllServiceModel> {}
}
