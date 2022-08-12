namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public string Flag { get; set; }

        [Required]
        public string RegistryPort { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
