using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalOffice.Models
{
    public class PatientPhoto : UploadedPhoto
    {
        [Display(Name = "Patient")]
        public int PatientID { get; set; }

        public Patient Patient { get; set; }
    }
}
