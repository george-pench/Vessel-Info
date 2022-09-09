namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public abstract class VesselBaseServiceModel : IMapFrom<Vessel>, IMapTo<Vessel>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Imo { get; set; }

        public string Built { get; set; }

        public string SummerDwt { get; set; }
    }
}
