using Microsoft.EntityFrameworkCore;
using System;

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
                mb.HasData(new Doctor[]
                {
                    new Doctor {IdDoctor = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jk@mail.com"},
                    new Doctor {IdDoctor = 2, FirstName = "Maria", LastName = "Antonina", Email = "ma@mail.com"},
                    new Doctor {IdDoctor = 3, FirstName = "Zygmunt", LastName = "Dobry", Email = "zd@mail.com"}
                });
            });

            modelBuilder.Entity<Patient>(mb =>
            {
                mb.HasKey(p => p.IdPatient);
                mb.Property(p => p.IdPatient).UseIdentityColumn();
                mb.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
                mb.Property(p => p.LastName).HasMaxLength(100).IsRequired();
                mb.Property(p => p.BirthDate).IsRequired();
                mb.ToTable("Patient");
                mb.HasData(new Patient[]
                {
                    new Patient {IdPatient = 1, FirstName = "Anna", LastName = "lato", BirthDate = new DateTime(1994, 12, 1)},
                    new Patient {IdPatient = 2, FirstName = "Stanisłąw", LastName = "Nowak", BirthDate = new DateTime(1957, 1, 25)},
                    new Patient {IdPatient = 3, FirstName = "Paweł", LastName = "Baran", BirthDate = new DateTime(1976, 6, 15)}
                });
            });

            modelBuilder.Entity<Medicament>(mb =>
            {
                mb.HasKey(m => m.IdMedicament);
                mb.Property(m => m.IdMedicament).UseIdentityColumn();
                mb.Property(m => m.Name).HasMaxLength(100).IsRequired();
                mb.Property(m => m.Description).HasMaxLength(100).IsRequired();
                mb.Property(m => m.Type).HasMaxLength(100).IsRequired();
                mb.ToTable("Medicament");
                mb.HasData(new Medicament[]
                {
                    new Medicament {IdMedicament = 1, Name = "Super piguł", Description = "Lek na wszystyko i jeszcze więcej", Type = "Pigułki"},
                    new Medicament {IdMedicament = 2, Name = "Super maść", Description = "Jeszcze nie wiadomo po co", Type = "Maść"}
                });
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
                mb.HasData(new Prescription[]
                {
                    new Prescription { IdPrescription = 1, Date = DateTime.Today, DueDate = new DateTime(2020, 10, 1), IdDoctor = 1, IdPatient = 2},
                    new Prescription { IdPrescription = 2, Date = DateTime.Today, DueDate = new DateTime(2020, 6, 16), IdDoctor = 2, IdPatient = 2},
                    new Prescription { IdPrescription = 3, Date = DateTime.Today, DueDate = new DateTime(2021, 5, 24), IdDoctor = 1, IdPatient = 3},
                    new Prescription { IdPrescription = 4, Date = DateTime.Today, DueDate = new DateTime(2020, 10, 1), IdDoctor = 2, IdPatient = 3},
                    new Prescription { IdPrescription = 5, Date = DateTime.Today, DueDate = new DateTime(2022, 3, 11), IdDoctor = 1, IdPatient = 2}
                });
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
                mb.HasData(new PrescriptionMedicament[]
                {
                    new PrescriptionMedicament {IdMedicament = 2, IdPrescription = 1, Details = "Może pomoże"},
                    new PrescriptionMedicament {IdMedicament = 1, IdPrescription = 2, Dose = 2,  Details = "2 x na dzień"},
                    new PrescriptionMedicament {IdMedicament = 2, IdPrescription = 2, Dose = 10,  Details = "Ile się da"},
                    new PrescriptionMedicament {IdMedicament = 1, IdPrescription = 3,  Details = "Na jutro"},
                    new PrescriptionMedicament {IdMedicament = 1, IdPrescription = 4, Dose = 20, Details = "Pa maści"},
                    new PrescriptionMedicament {IdMedicament = 2, IdPrescription = 4,  Details = "Po pigułach"}
                });
            });
        }
    }
}
