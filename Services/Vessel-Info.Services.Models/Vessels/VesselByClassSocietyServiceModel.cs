namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Services.Models.ClassSocieties;
    
    public class VesselByClassSocietyServiceModel : VesselBaseServiceModel
    {
        public ClassSocietyBaseServiceModel VesselClassSociety { get; set; }
    }
}
