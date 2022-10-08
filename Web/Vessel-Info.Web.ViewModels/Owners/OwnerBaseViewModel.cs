namespace Vessel_Info.Web.ViewModels.Owners
{
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    using static Constants.WebConstants.Owner;

    public class OwnerBaseViewModel : IMapFrom<VesselOwnerServiceModel>, IMapTo<VesselOwnerServiceModel>
    {
        public int Id { get; set; }

        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }
    }
}
