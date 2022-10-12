namespace Vessel_Info.Services.Models.Registrations
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class RegistrationBaseServiceModel : IMapFrom<Registration>, IMapTo<Registration>
    {
        public int Id { get; set; }

        public string Flag { get; set; }

        public string RegistryPort { get; set; }
    }
}
