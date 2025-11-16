using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
          : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique();

            modelBuilder.Entity<Patient>()
              .HasOne(p => p.User)
              .WithOne()
              .HasForeignKey<Patient>(p => p.UserId)
              .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Doctor>()
              .HasOne(d => d.User)
              .WithOne()
              .HasForeignKey<Doctor>(d => d.UserId)
              .OnDelete((DeleteBehavior.Cascade));
            modelBuilder.Entity<Appointment>()
              .HasOne(a => a.Patient)
              .WithMany()
              .HasForeignKey(a => a.PatientId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>()
              .HasOne(a => a.Doctor)
              .WithMany(d => d.Appointments)
              .HasForeignKey(a => a.DoctorId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MedicalRecord>()
              .HasOne(m => m.Patient)
              .WithMany(p => p.MedicalRecords)
              .HasForeignKey(m => m.PatientId)
              .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
              .Property(a => a.Status)
              .HasDefaultValue("Booked");
            modelBuilder.Entity<MedicalRecord>()
              .Property(m => m.CreatedAt)
              .HasDefaultValueSql("GETDATE()");


        }

    }
}
