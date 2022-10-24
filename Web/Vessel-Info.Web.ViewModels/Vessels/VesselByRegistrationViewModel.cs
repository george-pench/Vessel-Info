namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Web.ViewModels.Registrations;

    public class VesselByRegistrationViewModel : VesselBaseViewModel, IMapFrom<VesselByRegistrationServiceModel>
    {
        public RegistrationBaseViewModel VesselRegistration { get; set; }
    }
}
