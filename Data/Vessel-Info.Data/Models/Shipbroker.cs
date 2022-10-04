namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Shipbroker;

    public class Shipbroker
    {
        public int Id { get; set; }

        [MaxLength(AgencyNameMaxLength)]
        public string AgencyName { get; set; }

        [MaxLength(TelephoneNumberMaxLength)]
        public string TelephoneNumber { get; set; }

        public string UserId { get; set; }

        public IEnumerable<Vessel> Vessels { get; set; } = new List<Vessel>();

        public IEnumerable<ShipbrokerOperator> Operators { get; set; } = new List<ShipbrokerOperator>();
    }
}
