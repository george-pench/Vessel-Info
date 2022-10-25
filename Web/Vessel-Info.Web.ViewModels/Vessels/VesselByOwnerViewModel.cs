namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Web.ViewModels.Owners;

    public class VesselByOwnerViewModel : VesselBaseViewModel, IMapFrom<VesselByOwnerServiceModel>
    {
        public OwnerBaseViewModel VesselOwner { get; set; }
    }
}
