namespace Vessel_Info.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.ClassificationSociety;

    public class ClassificationSociety
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(AbbreviationMaxLength)]
        public string Abbreviation { get; set; }

        [MaxLength(FoundedMaxValue)]
        public DateTime Founded { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
