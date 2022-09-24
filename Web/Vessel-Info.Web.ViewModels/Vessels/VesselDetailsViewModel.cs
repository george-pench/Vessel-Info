namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselDetailsViewModel : VesselAllViewModel, IMapFrom<VesselDetailsServiceModel>
    {
        public string RegistrationFlag { get; set; }

        public string RegistrationRegistryPort { get; set; }

        public string TypeName { get; set; }

        public string ClassificationSocietyFullName { get; set; }

        public string OwnerName { get; set; }
    }
}
