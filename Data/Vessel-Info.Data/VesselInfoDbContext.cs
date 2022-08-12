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

        public DbSet<Type> Owners { get; set; }

        public DbSet<Type> Registrations { get; set; }

        public DbSet<Type> ClassificationSocieties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Vessel>()
                .HasOne(v => v.Type)
                .WithMany(t => t.Vessels)
                .HasForeignKey(v => v.TypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vessel>()
                .HasOne(v => v.Owner)
                .WithMany(o => o.Vessels)
                .HasForeignKey(v => v.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Vessel>()
                .HasOne(v => v.Registration)
                .WithMany(r => r.Vessels)
                .HasForeignKey(v => v.RegistrationId)
                .OnDelete(DeleteBehavior.Restrict);            

            builder
                .Entity<Vessel>()
                .HasOne(v => v.ClassificationSociety)
                .WithMany(cs => cs.Vessels)
                .HasForeignKey(v => v.ClassificationSocietyId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
