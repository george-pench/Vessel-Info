namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class VesselCreateServiceModel : IMapFrom<Vessel>, IMapTo<Vessel>
    {
        public VesselRegistrationServiceModel Registration { get; set; }

        public VesselTypeServiceModel Type { get; set; }

        public VesselClassificationSocietyServiceModel ClassificationSociety { get; set; }

        public VesselOwnerServiceModel Owner { get; set; }

        public VesselAllServiceModel Vessel { get; set; }
    }
}
