using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalOffice.Models
{
    public class Patient : Auditable, IValidatableObject
    {
        public Patient()
        {
            this.Appointments = new HashSet<Appointment>();
            this.PatientConditions = new HashSet<PatientCondition>();
            PatientDocuments = new HashSet<PatientDocument>();
        }

        public int ID { get; set; }

        [Display(Name = "Patient")]
        public string FullName
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }
        public string Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int? a = today.Year - DOB?.Year
                    - ((today.Month < DOB?.Month || (today.Month == DOB?.Month && today.Day < DOB?.Day) ? 1 : 0));
                return a?.ToString().PadLeft(2); 
            }
        }
        public string AgeSummary
        {
            get
            {
                if(string.IsNullOrWhiteSpace(Age))
                {
                    return "N/A";
                }
                else
                {
                    return Age + " (" + DOB.GetValueOrDefault().ToString("yyyy-MM-dd") + ")";
                }
            }
        }

        [Display(Name = "Med. Trial")]
        public string InTrial
        {
            get
            {
                return (MedicalTrialID.HasValue ? "Yes" : "No");
            }
        }

        [Required(ErrorMessage = "You cannot leave the OHIP number blank.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "The OHIP number must be exactly 10 numeric digits.")]
        [StringLength(10)]//DS Note: we only include this to limit the size of the database field to 10
        public string OHIP { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You cannot leave the first name blank.")]
        [StringLength(50, ErrorMessage = "First name cannot be more than 50 characters long.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle name cannot be more than 50 characters long.")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You cannot leave the last name blank.")]
        [StringLength(100, ErrorMessage = "Last name cannot be more than 100 characters long.")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Visits/Yr")]
        [Required(ErrorMessage = "You cannot leave the number of expected vists per year blank.")]
        [Range(1, 12, ErrorMessage = "The number of expected vists per year must be between 1 and 12.")]
        public byte ExpYrVisits { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression("^\\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number (no spaces).")]
        [DataType(DataType.PhoneNumber)]
        [DisplayFormat(DataFormatString = "{0:(###) ###-####}", ApplyFormatInEditMode = false)]
        public Int64 Phone { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string eMail { get; set; }

        [ScaffoldColumn(false)]
        [Timestamp]
        public Byte[] RowVersion { get; set; }//Added for concurrency

        [Required(ErrorMessage = "You must select a Primary Care Physician.")]
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }

        public Doctor Doctor { get; set; }

        [Display(Name = "Medical Trial")]
        public int? MedicalTrialID { get; set; }

        [Display(Name = "Medical Trial")]
        public MedicalTrial MedicalTrial { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        [Display(Name = "History")]
        public ICollection<PatientCondition> PatientConditions { get; set; }

        public ICollection<PatientDocument> PatientDocuments { get; set; }

        public PatientPhoto PatientPhoto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB.GetValueOrDefault()> DateTime.Today)
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }
        }
    }
}
