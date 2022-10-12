namespace Vessel_Info.Web.ViewModels.Types
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Types;

    using static Constants.WebConstants.Type;

    public class TypeBaseViewModel : IMapFrom<TypeBaseServiceModel>, IMapTo<TypeBaseServiceModel>
    {
        public int Id { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
