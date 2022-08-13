namespace Vessel_Info.Data.Models
{
    public class ShipbrokerOperator
    {
        public int ShipbrokerId { get; set; }

        public Shipbroker Shipbroker { get; set; }

        public int OperatorId { get; set; }

        public Operator Operator { get; set; }
    }
}
