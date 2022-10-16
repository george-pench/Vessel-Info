namespace Vessel_Info.Web.ViewModels.Operators
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Operators;

    using static Constants.WebConstants.Operator;

    public class OperatorDetailsViewModel : OperatorBaseViewModel, IMapFrom<OperatorBaseServiceModel>, IMapFrom<OperatorDetailsServiceModel>
    {
        [Range(FoundedMinValue, FoundedMaxValue)]
        public string Founded { get; set; }

        [StringLength(WebsiteMaxLength, MinimumLength = WebsiteMinLength)]
        public string Website { get; set; }
    }
}
