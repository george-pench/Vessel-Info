namespace Vessel_Info.Web.ViewModels.Operators
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Operators;

    using static Constants.WebConstants.Operator;

    public class OperatorBaseViewModel : IMapFrom<OperatorBaseServiceModel>, IMapTo<OperatorBaseServiceModel>
    {
        public int Id { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
