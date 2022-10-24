namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Services.Models.Registrations;
    
    public class VesselByRegistrationServiceModel : VesselBaseServiceModel
    {
        public RegistrationBaseServiceModel VesselRegistration { get; set; }
    }
}
