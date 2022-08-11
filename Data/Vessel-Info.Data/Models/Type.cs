namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Type
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
