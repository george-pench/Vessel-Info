namespace Vessel_Info.Services.Models.Vessels
{
    public class VesselCreateServiceModel
    {
        public int TypeId { get; set; }

        public int RegistrationId { get; set; }

        public int ClassificationSocietyId { get; set; }

        public int OwnerId { get; set; }

        public VesselAllServiceModel Vessel { get; set; }
    }
}
