namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Owner;

    public class Owner
    {
        public int Id { get; set; }

        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(FoundedMaxValue)]
        public string Founded { get; set; }

        [MaxLength(WebsiteMaxLength)]
        public string Website { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
