using FarmatikoData.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData
{
    public class FarmatikoDataContext : DbContext
    {
        public FarmatikoDataContext(DbContextOptions options) : base(options) { }


        public DbSet<HealthFacility> HealthFacilities { get; set; }
        public DbSet<HealthcareWorker> HealthcareWorkers { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyHead> PharmacyHeads { get; set; }
        public DbSet<Pandemic> Pandemics { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<RequestPharmacyHead> PHRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PharmacyHeadMedicine> PharmacyHeadMedicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<PharmacyHead>()
                .ToTable("PharmacyHeads");

            modelBuilder.Entity<Medicine>()
                .ToTable("Medicines");

            modelBuilder.Entity<Pharmacy>()
                .ToTable("Pharmacies");

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .ToTable("PharmacyHeadMedicines");

            modelBuilder.Entity<RequestPharmacyHead>()
                .ToTable("PHRequests");

            modelBuilder.Entity<Medicine>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<Pharmacy>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<PharmacyHead>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<RequestPharmacyHead>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            /*modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasKey(phm => new { phm.PheadId, phm.MedicineId });

            modelBuilder.Entity<PharmacyHead>()
                .HasMany<Pharmacy>(p => p.Pharmacy)
                .WithOne(p => p.PharmacyHead)
                .HasForeignKey();

            modelBuilder.Entity<Pharmacy>()
                .HasOne<PharmacyHead>(p => p.PharmacyHead)
                .WithMany(p => p.Pharmacy);
            */
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
