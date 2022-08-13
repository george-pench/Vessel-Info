namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.User;

    public class Shipbroker
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public string TelephoneNumber { get; set; }

        public string Agency { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
