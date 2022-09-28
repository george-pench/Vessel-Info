namespace Vessel_Info.Web.ViewModels.Vessels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselCreateInputModel : IMapTo<VesselCreateServiceModel>
    {
        [Display(Name = "Flag")]
        public int RegistrationId { get; set; }

        public IEnumerable<VesselRegistrationServiceModel> Registrations { get; set; }

        [Display(Name = "Class Society")]
        public int ClassificationSocietyId { get; set; }

        public IEnumerable<VesselClassificationSocietyServiceModel> ClassificationSocieties { get; set; }

        [Display(Name = "Owner")]
        public int OwnerId { get; set; }

        public IEnumerable<VesselOwnerServiceModel> Owners { get; set; }

        [Display(Name = "Type")]
        public int TypeId { get; set; }

        public IEnumerable<VesselTypeServiceModel> Types { get; set; }

        public VesselAllViewModel Vessel { get; set; }
    }
}
