namespace Vessel_Info.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class Operator
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ushort Founded { get; set; }

        public string Website { get; set; }

        public IEnumerable<ShipbrokerOperator> Shipbrokers { get; set; } = new List<ShipbrokerOperator>();
    }
}
