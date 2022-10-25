namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Services.Models.Owners;
    
    public class VesselByOwnerServiceModel : VesselBaseServiceModel
    {
        public OwnerBaseServiceModel VesselOwner { get; set; }
    }
}
