namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;

    using static Constants.WebConstants.ClassificationSociety;

    public class ClassSocietyDetailsViewModel : ClassSocietyBaseViewModel, IMapFrom<ClassSocietyDetailsServiceModel>
    {
        [StringLength(AbbreviationMaxLength, MinimumLength = AbbreviationMinLength)]
        public string Abbreviation { get; set; }

        [Range(FoundedMinValue, FoundedMaxValue)]
        public string Founded { get; set; }

        [StringLength(WebsiteMaxLength, MinimumLength = WebsiteMinLength)]
        public string Website { get; set; }
    }
}
