namespace Vessel_Info.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Vessel_Info.Data.Models;

    public class VesselInfoDbContext : IdentityDbContext<User>
    {
        public VesselInfoDbContext(DbContextOptions<VesselInfoDbContext> options) 
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
