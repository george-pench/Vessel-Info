namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Owner
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ushort Founded { get; set; }

        public string Website { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
