namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;

    using static Constants.WebConstants.ClassificationSociety;

    public class ClassSocietyBaseViewModel : IMapFrom<ClassSocietyBaseServiceModel>, IMapTo<ClassSocietyBaseServiceModel>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(FullNameMaxLength, MinimumLength = FullNameMinLength)]
        public string FullName { get; set; }
    }
}
