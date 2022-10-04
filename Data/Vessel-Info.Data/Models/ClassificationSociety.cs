namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.ClassificationSociety;

    public class ClassificationSociety
    {
        public int Id { get; set; }

        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [MaxLength(AbbreviationMaxLength)]
        public string Abbreviation { get; set; }

        [MaxLength(FoundedMaxValue)]
        public string Founded { get; set; }

        [MaxLength(WebsiteMaxLength)]
        public string Website { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
