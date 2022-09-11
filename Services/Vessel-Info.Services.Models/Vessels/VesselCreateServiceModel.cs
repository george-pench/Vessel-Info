namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class VesselCreateServiceModel : IMapFrom<Vessel>, IMapTo<Vessel>
    {
        public int TypeId { get; set; }

        public int RegistrationId { get; set; }

        public int ClassificationSocietyId { get; set; }

        public int OwnerId { get; set; }

        public VesselAllServiceModel Vessel { get; set; }
    }
}
