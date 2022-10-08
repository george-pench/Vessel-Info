namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.ClassificationSociety;

    public class ClassSocietyDetailsViewModel : ClassSocietyBaseViewModel, IMapFrom<VesselClassificationSocietyServiceModel>
    {
        [StringLength(AbbreviationMaxLength, MinimumLength = AbbreviationMinLength)]
        public string Abbreviation { get; set; }

        [Range(FoundedMinValue, FoundedMaxValue)]
        public string Founded { get; set; }

        [StringLength(WebsiteMinLength, MinimumLength = WebsiteMaxLength)]
        public string Website { get; set; }
    }
}
