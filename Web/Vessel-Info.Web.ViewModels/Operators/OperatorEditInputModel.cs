namespace Vessel_Info.Web.ViewModels.Operators
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Operators;

    public class OperatorEditInputModel : OperatorDetailsViewModel,
        IMapFrom<OperatorAllServiceModel>, IMapTo<OperatorEditServiceModel> { }
}
