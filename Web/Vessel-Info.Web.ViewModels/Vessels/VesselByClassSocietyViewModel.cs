namespace Vessel_Info.Web.ViewModels.Vessels
{
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;
    using Vessel_Info.Web.ViewModels.ClassSocieties;

    public class VesselByClassSocietyViewModel : VesselBaseViewModel, IMapFrom<VesselByClassSocietyServiceModel>
    {
        public ClassSocietyBaseViewModel VesselClassSociety { get; set; }
    }
}
