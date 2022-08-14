namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Operator;

    public class Operator
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [MaxLength(FoundedMaxValue)]
        public ushort Founded { get; set; }

        [MaxLength(WebsiteMaxLength)]
        public string Website { get; set; }

        public IEnumerable<ShipbrokerOperator> Shipbrokers { get; set; } = new List<ShipbrokerOperator>();
    }
}
