namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.ClassificationSociety;

    public class ClassSocietyBaseViewModel : IMapFrom<VesselClassificationSocietyServiceModel>, IMapTo<VesselClassificationSocietyServiceModel>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }
    }
}
