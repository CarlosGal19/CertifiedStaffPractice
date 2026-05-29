using Microsoft.EntityFrameworkCore;
using CertifiedStaff.Models;

namespace CertifiedStaff.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }

        public DbSet<ProductionLine> ProductionLines { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<ProductionLineStation> ProductionLineStations { get; set; }

        public DbSet<Certification> Certifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // SHIFT
            // =========================
            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(s => s.ShiftId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Shift>()
                .HasMany(s => s.Employees)
                .WithOne(e => e.Shift)
                .HasForeignKey(e => e.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Shift>()
                .HasMany(s => s.Supervisors)
                .WithOne(su => su.Shift)
                .HasForeignKey(su => su.ShiftId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // PRODUCTION LINE
            // =========================
            modelBuilder.Entity<ProductionLine>(entity =>
            {
                entity.HasKey(p => p.ProductionLineId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.IsActive).HasDefaultValue(true);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ProductionLine>()
                .HasMany(pl => pl.ProductionLineStations)
                .WithOne(ps => ps.ProductionLine)
                .HasForeignKey(ps => ps.ProductionLineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductionLine>()
                .HasMany(pl => pl.Supervisors)
                .WithOne(ps => ps.ProductionLine)
                .HasForeignKey(ps => ps.ProductionLineId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // SUPERVISOR
            // =========================
            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.HasKey(s => s.SupervisorId);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.IsActive).HasDefaultValue(true);
                entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // =========================
            // STATION
            // =========================
            modelBuilder.Entity<Station>(entity =>
            {
                entity.HasKey(s => s.StationId);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.IsActive).HasDefaultValue(true);
                entity.Property(s => s.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Station>()
                .HasMany(s => s.ProductionLineStations)
                .WithOne(ps => ps.Station)
                .HasForeignKey(ps => ps.StationId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // PRODUCTION LINE STATION
            // =========================
            modelBuilder.Entity<ProductionLineStation>(entity =>
            {
                entity.HasKey(ps => ps.ProductionLineStationId);
                entity.Property(ps => ps.IsActive).HasDefaultValue(true);
                entity.Property(ps => ps.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<ProductionLineStation>()
                .HasOne(ps => ps.ProductionLine)
                .WithMany(pl => pl.ProductionLineStations)
                .HasForeignKey(ps => ps.ProductionLineId);

            modelBuilder.Entity<ProductionLineStation>()
                .HasOne(ps => ps.Station)
                .WithMany(s => s.ProductionLineStations)
                .HasForeignKey(ps => ps.StationId);

            modelBuilder.Entity<ProductionLineStation>()
                .HasMany(ps => ps.Employees)
                .WithOne(e => e.ProductionLineStation)
                .HasForeignKey(e => e.ProductionLineStationId);

            // =========================
            // EMPLOYEE
            // =========================
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // =========================
            // CERTIFICATION
            // =========================
            modelBuilder.Entity<Certification>(entity =>
            {
                entity.HasKey(c => c.CertificationId);

                entity.Property(c => c.TrainingPercentage).IsRequired();
                entity.Property(c => c.CertificationDate);
                entity.Property(c => c.ExpirationDate);

                entity.Property(c => c.IsActive).HasDefaultValue(true);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                // Employee relationship
                entity.HasOne(c => c.Employee)
                      .WithMany(e => e.Certifications)
                      .HasForeignKey(c => c.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);

                // ProductionLineStation relationship
                entity.HasOne(c => c.ProductionLineStation)
                      .WithMany(ps => ps.Certifications)
                      .HasForeignKey(c => c.ProductionLineStationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
