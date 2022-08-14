namespace Vessel_Info.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.Vessel;

    public class Vessel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(ExNameMaxLength)]
        public string ExName { get; set; }
        
        [MaxLength(HyllTypeMaxLength)]
        public string HullType { get; set; }

        [Required]
        [MaxLength(ImoLength)]
        public int IMO { get; set; }

        [Required]
        [MaxLength(CallSignLength)]
        public string CallSign { get; set; }

        [Required]
        [MaxLength(SummertDwtNameMaxLength)]
        public int SummerDwt { get; set; }

        [Required]
        [MaxLength(BuiltMaxValue)]
        public DateTime Built { get; set; }

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
