namespace Vessel_Info.Data.Models
{
    using System;
    
    public class Vessel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int IMO { get; set; }

        public string CallSign { get; set; }

        public DateTime Built { get; set; }

        public int TypeId { get; set; }

        public Type Type { get; set; }
    }
}
