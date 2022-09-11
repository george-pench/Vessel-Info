namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselCreateInputModel : IMapFrom<VesselCreateServiceModel>, IMapTo<VesselCreateServiceModel>
    {
        public int RegistrationId { get; set; }

        public int TypeId { get; set; }

        public int ClassificationSocietyId { get; set; }

        public int OwnerId { get; set; }

        public VesselAllViewModel Vessel { get; set; }
    }
}
