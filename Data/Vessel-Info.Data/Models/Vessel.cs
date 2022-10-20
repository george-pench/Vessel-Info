namespace Vessel_Info.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Vessel;

    public class Vessel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ImoMaxLength)]
        public string Imo { get; set; }

        [MaxLength(BuiltMaxValue)]
        public string Built { get; set; }

        [MaxLength(SummertDwtMaxLength)]
        public string SummerDwt { get; set; }

        [MaxLength(LoaMaxLength)]
        public string Loa { get; set; }

        [MaxLength(CubicMaxLength)]
        public string Cubic { get; set; }

        [MaxLength(BeamMaxValue)]
        public string Beam { get; set; }

        [MaxLength(DraftMaxValue)]
        public string Draft { get; set; }

        [MaxLength(HyllTypeMaxLength)]
        public string HullType { get; set; }

        [MaxLength(CallSignMaxLength)]
        public string CallSign { get; set; }

        public int RegistrationId { get; set; }
        
        public Registration Registration { get; set; }

        public int TypeId { get; set; }

        public Type Type { get; set; }

        public int OwnerId { get; set; }

        public Owner Owner { get; set; }

        public int ClassificationSocietyId { get; set; }

        public ClassificationSociety ClassificationSociety { get; set; }

        public int ShipbrokerId { get; set; }

        public Shipbroker Shipbroker { get; set; }
    }
}
