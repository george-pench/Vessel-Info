namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselDetailsViewModel : VesselBaseViewModel, 
        IMapFrom<VesselDetailsServiceModel>, IMapTo<VesselDetailsServiceModel>
    {
        public string ExName { get; set; }

        public string Flag { get; set; }

        public string RegistryPort { get; set; }

        public string TypeName { get; set; }

        public string ClassificationSocietyFullName { get; set; }

        public string OwnerName { get; set; }
    }
}
