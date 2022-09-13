namespace Vessel_Info.Services.Models.Vessels
{
    public class VesselAllServiceModel : VesselBaseServiceModel
    {
        public string Loa { get; set; }

        public string Cubic { get; set; }

        public string Beam { get; set; }

        public string Draft { get; set; }

        public string HullType { get; set; }

        public string CallSign { get; set; }
    }
}
