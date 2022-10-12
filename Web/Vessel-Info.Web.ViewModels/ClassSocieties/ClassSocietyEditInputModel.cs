namespace Vessel_Info.Web.ViewModels.ClassSocieties
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;

    public class ClassSocietyEditInputModel : ClassSocietyDetailsViewModel, 
        IMapFrom<ClassSocietyAllServiceModel>, IMapTo<ClassSocietyEditServiceModel> { } 
}
