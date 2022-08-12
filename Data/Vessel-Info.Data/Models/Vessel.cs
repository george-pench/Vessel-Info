namespace Vessel_Info.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Vessel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ExName { get; set; }
        
        public string HullType { get; set; }

        [Required]
        public int IMO { get; set; }

        [Required]
        public string CallSign { get; set; }

        public int SummerDwt { get; set; }

        [Required]
        public DateTime Built { get; set; }

        public int RegistrationId { get; set; }

        public Registration Registration { get; set; }

        public int TypeId { get; set; }

        public Type Type { get; set; }

        public int OwnerId { get; set; }

        public Owner Owner { get; set; }

        public int ClassificationSocietyId { get; set; }

        public ClassificationSociety ClassificationSociety { get; set; }
    }
}
