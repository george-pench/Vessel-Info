﻿namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class VesselRegistrationServiceModel : IMapFrom<Registration>, IMapTo<Registration>
    {
        public string Flag { get; set; }

        public string RegistryPort { get; set; }
    }
}