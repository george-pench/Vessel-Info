namespace Vessel_Info.Web.ViewModels.Registrations
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Registration;

    public class RegistrationBaseViewModel : IMapFrom<VesselRegistrationServiceModel>, IMapTo<VesselRegistrationServiceModel>
    {
        public int Id { get; set; }

        [StringLength(FlagMaxLength, MinimumLength = FlagMinLength)]
        public string Flag { get; set; }

        [StringLength(RegistryPortMaxLength, MinimumLength = RegistryPortMinLength)]
        public string RegistryPort { get; set; }
    }
}
