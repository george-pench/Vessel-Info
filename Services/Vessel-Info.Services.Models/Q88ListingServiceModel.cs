namespace Vessel_Info.Services.Models
{
    using System.Collections.Generic;

    public class Q88ListingServiceModel
    {
        public List<string> Name { get; set; } = new List<string>();

        public List<string> Imo { get; set; } = new List<string>();

        public List<string> Built { get; set; } = new List<string>();

        public List<string> SummerDwt { get; set; } = new List<string>();

        public List<string> Loa { get; set; } = new List<string>();

        public List<string> Cubic { get; set; } = new List<string>();

        public List<string> Beam { get; set; } = new List<string>();

        public List<string> Draft { get; set; } = new List<string>();

        public List<string> HullType { get; set; } = new List<string>();

        public List<string> CallSign { get; set; } = new List<string>();
    }
}
