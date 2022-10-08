namespace Vessel_Info.Web.ViewModels.Owners
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Owner;

    public class OwnerDetailsViewModel : OwnerBaseViewModel, IMapFrom<VesselOwnerServiceModel>
    {
        [Range(FoundedMinValue, FoundedMaxValue)]
        public string Founded { get; set; }

        [StringLength(WebsiteMinLength, MinimumLength = WebsiteMaxLength)]
        public string Website { get; set; }
    }
}
