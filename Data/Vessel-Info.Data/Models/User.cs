namespace Vessel_Info.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
