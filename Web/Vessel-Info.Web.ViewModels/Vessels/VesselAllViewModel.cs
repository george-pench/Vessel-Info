namespace Vessel_Info.Web.ViewModels.Vessels
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Vessel;

    public class VesselAllViewModel : VesselBaseViewModel, 
        IMapFrom<VesselAllServiceModel>, IMapTo<VesselAllServiceModel>,
        IMapFrom<VesselCreateInputModel>, IMapTo<VesselCreateInputModel>
    {
        [Display(Name = "LOA")]
        [Range(LoaMinLength, LoaMaxLength)]
        public string Loa { get; set; }

        [Range(CubicMinLength, CubicMaxLength)]
        public string Cubic { get; set; }

        [Range(BeamMinValue, BeamMaxValue)]
        public string Beam { get; set; }

        [Range(DraftMinValue, DraftMaxValue)]
        public string Draft { get; set; }

        [Display(Name = "Hull Type")]
        [StringLength(HyllTypeMaxLength, MinimumLength = HullTypeMinLength)]
        public string HullType { get; set; }

        [Display(Name = "Call Sign")]
        [StringLength(CallSignMaxLength, MinimumLength = CallSignMinLength)]
        public string CallSign { get; set; }
    }
}
