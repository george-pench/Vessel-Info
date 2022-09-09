namespace Vessel_Info.Services.Models.Vessels
{
    public class VesselDetailsServiceModel : VesselAllServiceModel
    {
        public string ExName { get; set; }

        public VesselRegistrationServiceModel Registration { get; set; }

        public VesselTypeServiceModel Type { get; set; }

        public VesselClassificationSocietyServiceModel ClassificationSociety { get; set; }

        public VesselOwnerServiceModel Owner { get; set; }
    }
}
