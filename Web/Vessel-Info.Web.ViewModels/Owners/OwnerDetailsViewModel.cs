namespace Vessel_Info.Web.ViewModels.Owners
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Owners;

    using static Constants.WebConstants.Owner;

    public class OwnerDetailsViewModel : OwnerBaseViewModel, IMapFrom<OwnerBaseServiceModel>
    {
        [Range(FoundedMinValue, FoundedMaxValue)]
        public string Founded { get; set; }

        [StringLength(WebsiteMinLength, MinimumLength = WebsiteMaxLength)]
        public string Website { get; set; }
    }
}
