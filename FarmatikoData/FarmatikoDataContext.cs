using FarmatikoData.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmatikoData
{
    public class FarmatikoDataContext : DbContext
    {
        public FarmatikoDataContext(DbContextOptions options) : base(options) { }


        public virtual DbSet<HealthFacility> HealthFacilities { get; set; }
        public virtual DbSet<HealthcareWorker> HealthcareWorkers { get; set; }
        public virtual DbSet<Pharmacy> Pharmacies { get; set; }
        public virtual DbSet<PharmacyHead> PharmacyHeads { get; set; }
        public virtual DbSet<Pandemic> Pandemics { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<RequestPharmacyHead> PHRequests { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<PharmacyHeadMedicine> PharmacyHeadMedicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.Entity<PharmacyHead>()
                .ToTable("PharmacyHeads");

            modelBuilder.Entity<Medicine>()
                .ToTable("Medicines");

            modelBuilder.Entity<Pharmacy>()
                .ToTable("Pharmacies");

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

            modelBuilder.Entity<User>()
                .Property(x => x.Id)
                .HasIdentityOptions(startValue: 1);

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasKey(phm => new { phm.PheadId, phm.MedicineId });
            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasOne(ph => ph.Head)
                .WithMany(m => m.PHMedicineList)
                .HasForeignKey(k => k.PheadId);

            modelBuilder.Entity<PharmacyHeadMedicine>()
                .HasOne(m => m.Medicine)
                .WithMany(ml => ml.MedicineList)
                .HasForeignKey(k => k.MedicineId);

            modelBuilder.Entity<PharmacyHead>()
                .HasMany(p => p.PharmaciesList)
                .WithOne(h => h.PHead)
                .HasForeignKey(k => k.PheadId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
