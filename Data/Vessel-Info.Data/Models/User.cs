namespace Vessel_Info.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;

    using static Constants.DataConstants.User;

    public class User : IdentityUser
    {
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }
    }
}
