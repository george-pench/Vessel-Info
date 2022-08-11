namespace Vessel_Info.Data.Models
{
    using System;
    
    public class Vessel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ExName { get; set; }

        public int IMO { get; set; }

        public string CallSign { get; set; }

        public DateTime Built { get; set; }

        public int SummerDwt { get; set; }

        public string HullType { get; set; }

        public int TypeId { get; set; }

        public Type Type { get; set; }

        public int OwnerId { get; set; }

        public Owner Owner { get; set; }
    }
}
