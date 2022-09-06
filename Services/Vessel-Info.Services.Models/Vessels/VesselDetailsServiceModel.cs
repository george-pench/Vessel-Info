namespace Vessel_Info.Services.Models.Vessels
{  
    public class VesselDetailsServiceModel : VesselAllServiceModel
    {
        public string ExName { get; set; }

        public VesselRegistrationServiceModel Registration { get; set; }

        public VesselTypeServiceModel VesselType { get; set; }

        public VesselClassificationSocietyServiceModel ClassSociety { get; set; }

        public VesselOwnerServiceModel Owner { get; set; }
    }
}
