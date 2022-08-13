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

        public DbSet<Owner> Owners { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        public DbSet<ClassificationSociety> ClassificationSocieties { get; set; }

        public DbSet<Shipbroker> Shipbrokers { get; set; }
        
        public DbSet<Operator> Operators { get; set; }

        public DbSet<ShipbrokerOperator> ShipbrokersOperators { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ShipbrokerOperator>()
                .HasKey(so => new { so.OperatorId, so.ShipbrokerId });

            builder
                .Entity<ShipbrokerOperator>()
                .HasOne(so => so.Shipbroker)
                .WithMany(sb => sb.Operators)
                .HasForeignKey(so => so.ShipbrokerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ShipbrokerOperator>()
                .HasOne(so => so.Operator)
                .WithMany(o => o.Shipbrokers)
                .HasForeignKey(so => so.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

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

            builder
                .Entity<Shipbroker>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Shipbroker>(sb => sb.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
