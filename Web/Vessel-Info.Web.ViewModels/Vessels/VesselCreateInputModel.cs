namespace Vessel_Info.Web.ViewModels.Vessels
{
    public class VesselCreateInputModel
    {
        public int TypeId { get; set; }

        public int RegistrationId { get; set; }

        public int ClassificationSocietyId { get; set; }

        public int OwnerId { get; set; }

        public VesselAllViewModel Vessel { get; set; }
    }
}
