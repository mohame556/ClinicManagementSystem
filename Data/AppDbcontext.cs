using Clinc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinc.Data
{
    public class AppDbcontext : IdentityDbContext<AppUser>
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options ) : base(options)
        {
                
        }
        public DbSet<Patients> Patients { get; set; }

        public DbSet<Doctors> Doctors { get; set; }

        public DbSet<MedicalHistory> MedicalHistories { get; set; }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<License> Licenses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Transaction)
                .WithOne(t => t.Appointment)
                .HasForeignKey<Transaction>(t => t.AppointmentId);
        }
    }
}
