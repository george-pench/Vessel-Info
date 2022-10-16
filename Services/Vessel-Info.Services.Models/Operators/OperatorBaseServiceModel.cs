namespace Vessel_Info.Services.Models.Operators
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class OperatorBaseServiceModel : IMapFrom<Operator>, IMapTo<Operator>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
