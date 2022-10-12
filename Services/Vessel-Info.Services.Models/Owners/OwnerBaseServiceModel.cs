namespace Vessel_Info.Services.Models.Owners
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class OwnerBaseServiceModel : IMapFrom<Owner>, IMapTo<Owner>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
