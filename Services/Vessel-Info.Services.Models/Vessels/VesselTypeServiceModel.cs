namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class VesselTypeServiceModel : IMapFrom<Type>, IMapTo<Type>
    {
        public string Name { get; set; }
    }
}
