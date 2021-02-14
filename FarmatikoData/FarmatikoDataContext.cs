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

            modelBuilder.Entity<PharmacyHead>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Medicine>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Pharmacy>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PharmacyHead>()
                .HasMany<Pharmacy>(p => p.Pharmacies)
                .WithOne(p => p.PharmacyHead);

            modelBuilder.Entity<Pharmacy>()
                .HasOne<PharmacyHead>(p => p.PharmacyHead)
                .WithMany(p => p.Pharmacies)
                .HasForeignKey(x => x.PheadId);

            modelBuilder.Entity<PharmacyHeadMedicine>()
            .HasKey(bc => new { bc.PheadId, bc.MedicineId});

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasOne(bc => bc.Head)
                .WithMany(b => b.Medicines)
                .HasForeignKey(bc => bc.PheadId);

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasOne(bc => bc.Medicine)
                .WithMany(c => c.Medicines)
                .HasForeignKey(bc => bc.MedicineId);


            base.OnModelCreating(modelBuilder);
        }
    }
}
