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
        public string Loa { get; set; }

        public string Cubic { get; set; }

        public string Beam { get; set; }

        public string Draft { get; set; }

        [Display(Name = "Hull Type")]
        public string HullType { get; set; }

        [Display(Name = "Call Sign")]
        public string CallSign { get; set; }
    }
}
