﻿namespace Vessel_Info.Services.Models.Vessels
{
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;

    public class VesselClassificationSocietyServiceModel : IMapFrom<ClassificationSociety>, IMapTo<ClassificationSociety>
    {
        public string FullName { get; set; }
    }
}