namespace Vessel_Info.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Vessel_Info.Data.Models;

    public class VesselInfoDbContext : IdentityDbContext<User>
    {
        public VesselInfoDbContext(DbContextOptions<VesselInfoDbContext> options) 
            : base(options) {}

        public DbSet<Vessel> Vessels { get; set; }

        public DbSet<Type> Types { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
