namespace Vessel_Info.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class ClassificationSociety
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        public DateTime Founded { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
