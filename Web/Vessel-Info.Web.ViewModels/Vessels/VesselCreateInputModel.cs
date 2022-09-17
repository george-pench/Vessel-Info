namespace Vessel_Info.Web.ViewModels.Vessels
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Vessel;

    public class VesselCreateInputModel : IMapFrom<VesselCreateServiceModel>, IMapTo<VesselCreateServiceModel>
    {
        [Display(Name = "Registration ID")]
        public int RegistrationId { get; set; }

        [Display(Name = "Type ID")]
        public int TypeId { get; set; }

        [Display(Name = "Classification Society ID")]
        public int ClassificationSocietyId { get; set; }

        [Display(Name = "Owner ID")]
        public int OwnerId { get; set; }

        public VesselAllViewModel Vessel { get; set; }
    }
}
