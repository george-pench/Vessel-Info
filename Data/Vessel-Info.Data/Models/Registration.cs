namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Registration;
    
    public class Registration
    {
        public int Id { get; set; }

        [MaxLength(FlagMaxLength)]
        public string Flag { get; set; }

        [MaxLength(RegistryPortMaxLength)]
        public string RegistryPort { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();
    }
}
