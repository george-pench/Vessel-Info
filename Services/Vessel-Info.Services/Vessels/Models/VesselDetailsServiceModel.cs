﻿namespace Vessel_Info.Services.Vessels.Models
{
    public class VesselDetailsServiceModel : VesselAllServiceModel
    {
        public string ExName { get; set; }

        public VesselRegistrationServiceModel Registration { get; set; }

        public VesselTypeServiceModel VesselType { get; set; }

        public string HullType { get; set; }

        public VesselClassificationSocietyServiceModel ClassSociety { get; set; }

        public VesselOwnerServiceModel Owner { get; set; }
    }
}
