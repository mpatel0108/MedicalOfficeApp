﻿// <auto-generated />
using System;
using MedicalOffice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedicalOffice.Data.MOMigrations
{
    [DbContext(typeof(MedicalOfficeContext))]
    partial class MedicalOfficeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("MO")
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("MedicalOffice.Models.Appointment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ApptReasonID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(2000);

                    b.Property<int>("PatientID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("appDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("extraFee")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ApptReasonID");

                    b.HasIndex("PatientID");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("MedicalOffice.Models.ApptReason", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReasonName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("ApptReasons");
                });

            modelBuilder.Entity("MedicalOffice.Models.Condition", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConditionName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Conditions");
                });

            modelBuilder.Entity("MedicalOffice.Models.Doctor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("MedicalOffice.Models.DoctorSpecialty", b =>
                {
                    b.Property<int>("DoctorID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SpecialtyID")
                        .HasColumnType("INTEGER");

                    b.HasKey("DoctorID", "SpecialtyID");

                    b.HasIndex("SpecialtyID");

                    b.ToTable("DoctorSpecialties");
                });

            modelBuilder.Entity("MedicalOffice.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<string>("FavouriteIceCream")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("MedicalOffice.Models.FileContent", b =>
                {
                    b.Property<int>("FileContentID")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.HasKey("FileContentID");

                    b.ToTable("FileContent");
                });

            modelBuilder.Entity("MedicalOffice.Models.MedicalTrial", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TrialName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("MedicalTrials");
                });

            modelBuilder.Entity("MedicalOffice.Models.Patient", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("TEXT");

                    b.Property<int>("DoctorID")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ExpYrVisits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.Property<int?>("MedicalTrialID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("OHIP")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(10);

                    b.Property<long>("Phone")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("eMail")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.HasIndex("DoctorID");

                    b.HasIndex("MedicalTrialID");

                    b.HasIndex("OHIP")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientCondition", b =>
                {
                    b.Property<int>("ConditionID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PatientID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ConditionID", "PatientID");

                    b.HasIndex("PatientID");

                    b.ToTable("PatientConditions");
                });

            modelBuilder.Entity("MedicalOffice.Models.PhotoContent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Content")
                        .HasColumnType("BLOB");

                    b.Property<string>("MimeType")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<int?>("PhotoFullId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PhotoThumbId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("PhotoFullId")
                        .IsUnique();

                    b.HasIndex("PhotoThumbId")
                        .IsUnique();

                    b.ToTable("PhotoContent");
                });

            modelBuilder.Entity("MedicalOffice.Models.Specialty", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SpecialtyName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("MedicalOffice.Models.UploadedFile", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.Property<string>("MimeType")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.ToTable("UploadedFiles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UploadedFile");
                });

            modelBuilder.Entity("MedicalOffice.Models.UploadedPhoto", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("ID");

                    b.ToTable("UploadedPhotos");

                    b.HasDiscriminator<string>("Discriminator").HasValue("UploadedPhoto");
                });

            modelBuilder.Entity("MedicalOffice.Models.DoctorDocument", b =>
                {
                    b.HasBaseType("MedicalOffice.Models.UploadedFile");

                    b.Property<int>("DoctorID")
                        .HasColumnType("INTEGER");

                    b.HasIndex("DoctorID");

                    b.HasDiscriminator().HasValue("DoctorDocument");
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientDocument", b =>
                {
                    b.HasBaseType("MedicalOffice.Models.UploadedFile");

                    b.Property<int>("PatientID")
                        .HasColumnType("INTEGER");

                    b.HasIndex("PatientID");

                    b.HasDiscriminator().HasValue("PatientDocument");
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientPhoto", b =>
                {
                    b.HasBaseType("MedicalOffice.Models.UploadedPhoto");

                    b.Property<int>("PatientID")
                        .HasColumnType("INTEGER");

                    b.HasIndex("PatientID")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("PatientPhoto");
                });

            modelBuilder.Entity("MedicalOffice.Models.Appointment", b =>
                {
                    b.HasOne("MedicalOffice.Models.ApptReason", "ApptReason")
                        .WithMany("Appointments")
                        .HasForeignKey("ApptReasonID");

                    b.HasOne("MedicalOffice.Models.Patient", "Patient")
                        .WithMany("Appointments")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.DoctorSpecialty", b =>
                {
                    b.HasOne("MedicalOffice.Models.Doctor", "Doctor")
                        .WithMany("DoctorSpecialties")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MedicalOffice.Models.Specialty", "Specialty")
                        .WithMany("DoctorSpecialties")
                        .HasForeignKey("SpecialtyID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.FileContent", b =>
                {
                    b.HasOne("MedicalOffice.Models.UploadedFile", "UploadedFile")
                        .WithOne("FileContent")
                        .HasForeignKey("MedicalOffice.Models.FileContent", "FileContentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.Patient", b =>
                {
                    b.HasOne("MedicalOffice.Models.Doctor", "Doctor")
                        .WithMany("Patients")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicalOffice.Models.MedicalTrial", "MedicalTrial")
                        .WithMany("Patients")
                        .HasForeignKey("MedicalTrialID");
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientCondition", b =>
                {
                    b.HasOne("MedicalOffice.Models.Condition", "Condition")
                        .WithMany("PatientConditions")
                        .HasForeignKey("ConditionID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MedicalOffice.Models.Patient", "Patient")
                        .WithMany("PatientConditions")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.PhotoContent", b =>
                {
                    b.HasOne("MedicalOffice.Models.UploadedPhoto", "PhotoFull")
                        .WithOne("PhotoContentFull")
                        .HasForeignKey("MedicalOffice.Models.PhotoContent", "PhotoFullId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MedicalOffice.Models.UploadedPhoto", "PhotoThumb")
                        .WithOne("PhotoContentThumb")
                        .HasForeignKey("MedicalOffice.Models.PhotoContent", "PhotoThumbId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MedicalOffice.Models.DoctorDocument", b =>
                {
                    b.HasOne("MedicalOffice.Models.Doctor", "Doctor")
                        .WithMany("DoctorDocuments")
                        .HasForeignKey("DoctorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientDocument", b =>
                {
                    b.HasOne("MedicalOffice.Models.Patient", "Patient")
                        .WithMany("PatientDocuments")
                        .HasForeignKey("PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MedicalOffice.Models.PatientPhoto", b =>
                {
                    b.HasOne("MedicalOffice.Models.Patient", "Patient")
                        .WithOne("PatientPhoto")
                        .HasForeignKey("MedicalOffice.Models.PatientPhoto", "PatientID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
