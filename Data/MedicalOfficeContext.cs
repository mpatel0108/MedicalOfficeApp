﻿using MedicalOffice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedicalOffice.Data
{
    public class MedicalOfficeContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public MedicalOfficeContext(DbContextOptions<MedicalOfficeContext> options
            , IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            UserName = _httpContextAccessor.HttpContext?.User.Identity.Name;
            UserName = UserName ?? "Unknown";
        }

        public MedicalOfficeContext(DbContextOptions<MedicalOfficeContext> options)
            : base(options)
        {
            UserName = "SeedData";
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ApptReason> ApptReasons { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
        public DbSet<Condition> Conditions { get; set; }
        public DbSet<MedicalTrial> MedicalTrials { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<DoctorSpecialty> DoctorSpecialties { get; set; }
        public DbSet<DoctorDocument> DoctorDocuments { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<PatientPhoto> PatientPhotos { get; set; }
        public DbSet<PatientDocument> PatientDocuments { get; set; }
        public DbSet<UploadedPhoto> UploadedPhotos { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MO");

            //Add a unique index to the Employee Email
            modelBuilder.Entity<Employee>()
            .HasIndex(a => new { a.Email })
            .IsUnique();

            //Many to Many Intersection
            modelBuilder.Entity<PatientCondition>()
            .HasKey(t => new { t.ConditionID, t.PatientID });

            //Many to Many Doctor Specialty Primary Key
            modelBuilder.Entity<DoctorSpecialty>()
            .HasKey(t => new { t.DoctorID, t.SpecialtyID });

            //Add this so you DO get Cascade Delete
            modelBuilder.Entity<UploadedPhoto>()
                .HasOne<PhotoContent>(p => p.PhotoContentFull)
                .WithOne(p => p.PhotoFull)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UploadedPhoto>()
                .HasOne<PhotoContent>(p => p.PhotoContentThumb)
                .WithOne(p => p.PhotoThumb)
                .OnDelete(DeleteBehavior.Cascade);

            //Prevent Cascade Delete from Doctor to Patient
            //so we are prevented from deleting a Doctor with
            //Patients assigned
            modelBuilder.Entity<Doctor>()
                .HasMany<Patient>(d => d.Patients)
                .WithOne(p => p.Doctor)
                .HasForeignKey(p => p.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            //Add this so you don't get Cascade Delete
            modelBuilder.Entity<Specialty>()
                .HasMany<DoctorSpecialty>(p => p.DoctorSpecialties)
                .WithOne(c => c.Specialty)
                .HasForeignKey(c => c.SpecialtyID)
                .OnDelete(DeleteBehavior.Restrict);

            //Add this so you don't get Cascade Delete
            modelBuilder.Entity<PatientCondition>()
                .HasOne(pc => pc.Condition)
                .WithMany(c => c.PatientConditions)
                .HasForeignKey(pc => pc.ConditionID)
                .OnDelete(DeleteBehavior.Restrict);

            //Add a unique index to the OHIP Number
            modelBuilder.Entity<Patient>()
            .HasIndex(p => p.OHIP)
            .IsUnique();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}
