namespace Vessel_Info.Web.ViewModels.Vessels
{
    public abstract class VesselBaseViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Imo { get; set; }

        public string Built { get; set; }

        public string SummerDwt { get; set; }

        public string HullType { get; set; }

        public string CallSign { get; set; }
    }
}
