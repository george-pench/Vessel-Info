namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Services.Models.Types;
    
    public class VesselByTypeServiceModel : VesselBaseServiceModel
    {
        public TypeBaseServiceModel VesselType { get; set; }
    }
}
