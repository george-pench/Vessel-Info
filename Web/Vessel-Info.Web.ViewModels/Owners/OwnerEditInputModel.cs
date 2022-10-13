namespace Vessel_Info.Web.ViewModels.Owners
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Owners;

    public class OwnerEditInputModel : OwnerDetailsViewModel,
        IMapFrom<OwnerAllServiceModel>, IMapTo<OwnerEditServiceModel> { } 
}
