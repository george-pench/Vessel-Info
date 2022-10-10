namespace Vessel_Info.Web.ViewModels.Types
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Type;

    public class TypeBaseViewModel : IMapFrom<VesselTypeServiceModel>, IMapTo<VesselTypeServiceModel>
    {
        public int Id { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
