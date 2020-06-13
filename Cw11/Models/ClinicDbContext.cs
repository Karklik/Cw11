using Microsoft.EntityFrameworkCore;

namespace Cw11.Models
{
    public class ClinicDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        public ClinicDbContext() { }
        public ClinicDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>(mb =>
            {
                mb.HasKey(d => d.IdDoctor);
                mb.Property(d => d.IdDoctor).UseIdentityColumn();
                mb.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
                mb.Property(d => d.LastName).HasMaxLength(100).IsRequired();
                mb.Property(d => d.Email).HasMaxLength(100).IsRequired();
                mb.ToTable("Doctor");
            });

            modelBuilder.Entity<Patient>(mb =>
            {
                mb.HasKey(p => p.IdPatient);
                mb.Property(p => p.IdPatient).UseIdentityColumn();
                mb.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
                mb.Property(p => p.LastName).HasMaxLength(100).IsRequired();
                mb.Property(p => p.BirthDate).IsRequired();
                mb.ToTable("Patient");
            });

            modelBuilder.Entity<Medicament>(mb =>
            {
                mb.HasKey(m => m.IdMedicament);
                mb.Property(m => m.IdMedicament).UseIdentityColumn();
                mb.Property(m => m.Name).HasMaxLength(100).IsRequired();
                mb.Property(m => m.Description).HasMaxLength(100).IsRequired();
                mb.Property(m => m.Type).HasMaxLength(100).IsRequired();
                mb.ToTable("Medicament");
            });

            modelBuilder.Entity<Prescription>(mb =>
            {
                mb.HasKey(p => p.IdPrescription);
                mb.Property(p => p.IdPrescription).UseIdentityColumn();
                mb.Property(p => p.Date).IsRequired();
                mb.Property(p => p.DueDate).IsRequired();
                mb.HasOne(p => p.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(p => p.IdPatient)
                    .IsRequired();
                mb.HasOne(p => p.Doctor)
                    .WithMany(d => d.Prescriptions)
                    .HasForeignKey(p => p.IdDoctor)
                    .IsRequired();
                mb.ToTable("Prescription");
            });

            modelBuilder.Entity<PrescriptionMedicament>(mb =>
            {
                mb.HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
                mb.Property(pm => pm.Details).HasMaxLength(100).IsRequired();
                mb.HasOne(pm => pm.Medicament)
                    .WithMany(m => m.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdMedicament);
                mb.HasOne(pm => pm.Prescription)
                    .WithMany(p => p.PrescriptionMedicaments)
                    .HasForeignKey(pm => pm.IdPrescription);
                // Neded due missing index in migration :/
                mb.HasIndex(pm => pm.IdMedicament);
                mb.HasIndex(pm => pm.IdPrescription);
                mb.ToTable("Prescription_Medicament");
            });
        }
    }
}
