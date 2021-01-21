using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalOffice.Models
{
    public class Appointment
    {
        public Appointment()
        {
            //Example of setting defaults
            appDate = DateTime.Today;
            extraFee = 20m;
        }

        public int ID { get; set; }

        [Required(ErrorMessage = "You cannot leave the notes blank.")]
        [StringLength(2000, ErrorMessage = "Only 2000 characters for notes.")]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Required(ErrorMessage = "You cannot leave the date for the appointment blank.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime appDate { get; set; }

        [Required(ErrorMessage = "You must enter an amount for the extra fee.")]
        [Display(Name = "Extra Fee")]
        [DataType(DataType.Currency)]
        public decimal extraFee { get; set; }

        [Required(ErrorMessage = "You must select a Patient.")]
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        public virtual Patient Patient { get; set; }

        //Note: Reason is not required
        [Display(Name = "Reason for Appointment")]
        public int? ApptReasonID { get; set; }

        [Display(Name = "Reason for Appointment")]
        public ApptReason ApptReason { get; set; }
    }
}
