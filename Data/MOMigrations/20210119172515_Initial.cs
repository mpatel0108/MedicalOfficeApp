using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedicalOffice.Data.MOMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MO");

            migrationBuilder.CreateTable(
                name: "ApptReasons",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReasonName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApptReasons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConditionName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 10, nullable: true),
                    FavouriteIceCream = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MedicalTrials",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrialName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalTrials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpecialtyName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    OHIP = table.Column<string>(maxLength: 10, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    DOB = table.Column<DateTime>(nullable: true),
                    ExpYrVisits = table.Column<byte>(nullable: false),
                    Phone = table.Column<long>(nullable: false),
                    eMail = table.Column<string>(maxLength: 255, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    DoctorID = table.Column<int>(nullable: false),
                    MedicalTrialID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Patients_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalSchema: "MO",
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_MedicalTrials_MedicalTrialID",
                        column: x => x.MedicalTrialID,
                        principalSchema: "MO",
                        principalTable: "MedicalTrials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.Sql(
        @"
        CREATE TRIGGER SetPatientTimestampOnUpdate
        AFTER UPDATE ON Patients
        BEGIN
            UPDATE Patients
            SET RowVersion = randomblob(8)
            WHERE rowid = NEW.rowid;
        END
    ");
            migrationBuilder.Sql(
                @"
        CREATE TRIGGER SetPatientTimestampOnInsert
        AFTER INSERT ON Patients
        BEGIN
            UPDATE Patients
            SET RowVersion = randomblob(8)
            WHERE rowid = NEW.rowid;
        END
    ");
            migrationBuilder.CreateTable(
                name: "DoctorSpecialties",
                schema: "MO",
                columns: table => new
                {
                    DoctorID = table.Column<int>(nullable: false),
                    SpecialtyID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialties", x => new { x.DoctorID, x.SpecialtyID });
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalSchema: "MO",
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialties_Specialties_SpecialtyID",
                        column: x => x.SpecialtyID,
                        principalSchema: "MO",
                        principalTable: "Specialties",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Notes = table.Column<string>(maxLength: 2000, nullable: false),
                    appDate = table.Column<DateTime>(nullable: false),
                    extraFee = table.Column<decimal>(nullable: false),
                    PatientID = table.Column<int>(nullable: false),
                    ApptReasonID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Appointments_ApptReasons_ApptReasonID",
                        column: x => x.ApptReasonID,
                        principalSchema: "MO",
                        principalTable: "ApptReasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "MO",
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientConditions",
                schema: "MO",
                columns: table => new
                {
                    ConditionID = table.Column<int>(nullable: false),
                    PatientID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientConditions", x => new { x.ConditionID, x.PatientID });
                    table.ForeignKey(
                        name: "FK_PatientConditions_Conditions_ConditionID",
                        column: x => x.ConditionID,
                        principalSchema: "MO",
                        principalTable: "Conditions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientConditions_Patients_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "MO",
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MimeType = table.Column<string>(maxLength: 255, nullable: true),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    DoctorID = table.Column<int>(nullable: true),
                    PatientID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Doctors_DoctorID",
                        column: x => x.DoctorID,
                        principalSchema: "MO",
                        principalTable: "Doctors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadedFiles_Patients_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "MO",
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedPhotos",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(maxLength: 255, nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    PatientID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedPhotos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UploadedPhotos_Patients_PatientID",
                        column: x => x.PatientID,
                        principalSchema: "MO",
                        principalTable: "Patients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileContent",
                schema: "MO",
                columns: table => new
                {
                    FileContentID = table.Column<int>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileContent", x => x.FileContentID);
                    table.ForeignKey(
                        name: "FK_FileContent_UploadedFiles_FileContentID",
                        column: x => x.FileContentID,
                        principalSchema: "MO",
                        principalTable: "UploadedFiles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoContent",
                schema: "MO",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<byte[]>(nullable: true),
                    MimeType = table.Column<string>(maxLength: 255, nullable: true),
                    PhotoFullId = table.Column<int>(nullable: true),
                    PhotoThumbId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoContent", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhotoContent_UploadedPhotos_PhotoFullId",
                        column: x => x.PhotoFullId,
                        principalSchema: "MO",
                        principalTable: "UploadedPhotos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoContent_UploadedPhotos_PhotoThumbId",
                        column: x => x.PhotoThumbId,
                        principalSchema: "MO",
                        principalTable: "UploadedPhotos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ApptReasonID",
                schema: "MO",
                table: "Appointments",
                column: "ApptReasonID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                schema: "MO",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialties_SpecialtyID",
                schema: "MO",
                table: "DoctorSpecialties",
                column: "SpecialtyID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                schema: "MO",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientConditions_PatientID",
                schema: "MO",
                table: "PatientConditions",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DoctorID",
                schema: "MO",
                table: "Patients",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MedicalTrialID",
                schema: "MO",
                table: "Patients",
                column: "MedicalTrialID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_OHIP",
                schema: "MO",
                table: "Patients",
                column: "OHIP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoContent_PhotoFullId",
                schema: "MO",
                table: "PhotoContent",
                column: "PhotoFullId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoContent_PhotoThumbId",
                schema: "MO",
                table: "PhotoContent",
                column: "PhotoThumbId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_DoctorID",
                schema: "MO",
                table: "UploadedFiles",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedFiles_PatientID",
                schema: "MO",
                table: "UploadedFiles",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_UploadedPhotos_PatientID",
                schema: "MO",
                table: "UploadedPhotos",
                column: "PatientID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "DoctorSpecialties",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "FileContent",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "PatientConditions",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "PhotoContent",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "ApptReasons",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "UploadedFiles",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "Conditions",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "UploadedPhotos",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "Patients",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "Doctors",
                schema: "MO");

            migrationBuilder.DropTable(
                name: "MedicalTrials",
                schema: "MO");
        }
    }
}
