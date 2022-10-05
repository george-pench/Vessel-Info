namespace Vessel_Info.Web.ViewModels.Vessels
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.WebConstants.Vessel;

    public abstract class VesselBaseViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "IMO")]
        [StringLength(ImoMaxLength, MinimumLength = ImoMinLength)]
        public string Imo { get; set; }
        
        [Range(BuiltMinValue, BuiltMaxValue)]
        public string Built { get; set; }

        [Display(Name = "Summert DWT")]
        [Range(SummertDwtMinLength, SummertDwtMaxLength)]
        public string SummerDwt { get; set; }
    }
}
