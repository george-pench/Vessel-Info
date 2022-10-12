namespace Vessel_Info.Services.Models.Types
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class TypeBaseServiceModel : IMapFrom<Type>, IMapTo<Type>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
