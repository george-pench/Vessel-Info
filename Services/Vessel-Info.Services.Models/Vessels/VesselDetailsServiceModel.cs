namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Models.Registrations;
    using Vessel_Info.Services.Models.Types;

    public class VesselDetailsServiceModel : VesselAllServiceModel
    {
        public RegistrationBaseServiceModel Registration { get; set; }

        public TypeBaseServiceModel Type { get; set; }

        public ClassSocietyBaseServiceModel ClassificationSociety { get; set; }

        public OwnerBaseServiceModel Owner { get; set; }
    }
}
