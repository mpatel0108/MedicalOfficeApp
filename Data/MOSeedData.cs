using MedicalOffice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalOffice.Data
{
    public static class MOSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new MedicalOfficeContext(
                serviceProvider.GetRequiredService<DbContextOptions<MedicalOfficeContext>>()))
            {
                //Prepare Random
                Random random = new Random();

                // Look for any Patients.  Since we can't have patients without Doctors.
                if (!context.Doctors.Any())
                {
                    context.Doctors.AddRange(
                     new Doctor
                     {
                         FirstName = "Gregory",
                         MiddleName = "A",
                         LastName = "House"
                     },

                     new Doctor
                     {
                         FirstName = "Doogie",
                         MiddleName = "R",
                         LastName = "Houser"
                     },
                     new Doctor
                     {
                         FirstName = "Charles",
                         LastName = "Xavier"
                     }
                );
                    context.SaveChanges();
                }

                //Seed Specialties
                string[] specialties = new string[] { "Abdominal Radiology", "Addiction Psychiatry", "Adolescent Medicine Pediatrics", "Cardiothoracic Anesthesiology", "Adult Reconstructive Orthopaedics", "Advanced Heart Failure ", "Allergy & Immunology ", "Anesthesiology ", "Biochemical Genetics", "Blood Banking ", "Cardiothoracic Radiology", "Cardiovascular Disease Internal Medicine", "Chemical Pathology", "Child & Adolescent Psychiatry", "Child Abuse Pediatrics", "Child Neurology", "Clinical & Laboratory Immunology", "Clinical Cardiac Electrophysiology", "Clinical Neurophysiology Neurology", "Colon & Rectal Surgery ", "Congenital Cardiac Surgery", "Craniofacial Surgery", "Critical Care Medicine", "Cytopathology ", "Dermatology ", "Dermatopathology ", "Family Medicine ", "Family Practice", "Female Pelvic Medicine", "Foot & Ankle Orthopaedics", "Forensic Pathology", "Forensic Psychiatry ", "Hand Surgery", "Hematology Pathology", "Oncology ", "Infectious Disease", "Internal Medicine ", "Interventional Cardiology", "Neonatal-Perinatal Medicine", "Nephrology Internal Medicine", "Neurological Surgery ", "Neurology ", "Neuromuscular Medicine", "Neuromuscular Medicine", "Neuropathology Pathology", "Nuclear Medicine ", "Nuclear Radiology", "Obstetric Anesthesiology", "Obstetrics & Gynecology ", "Ophthalmic Plastic", "Ophthalmology ", "Orthopaedic Sports Medicine", "Orthopaedic Surgery ", "Otolaryngology ", "Otology", "Pediatrics ", "Plastic Surgery ", "Preventive Medicine ", "Radiation Oncology ", "Rheumatology", "Vascular & Interventional Radiology", "Vascular Surgery", "Integrated Thoracic Surgery", "Transplant Hepatology", "Urology" };
                if (!context.Specialties.Any())
                {
                    foreach (string s in specialties)
                    {
                        Specialty sp = new Specialty
                        {
                            SpecialtyName = s
                        };
                        context.Specialties.Add(sp);
                    }
                    context.SaveChanges();
                }
                //Create collection of the primary keys of the Specialties
                int[] specialtyIDs = context.Specialties.Select(s => s.ID).ToArray();
                int[] doctorIDs = context.Doctors.Select(d => d.ID).ToArray();

                //DoctorSpecialties - the Intersection
                //Add a few specialties to each Doctor
                if (!context.DoctorSpecialties.Any())
                {
                    int specialtyCount = specialtyIDs.Count();
                    foreach (int i in doctorIDs)
                    {
                        int howMany = random.Next(1, 4);
                        howMany = (howMany > specialtyCount) ? specialtyCount : howMany;
                        for (int j = 1; j <= howMany; j++)
                        {
                            DoctorSpecialty ds = new DoctorSpecialty()
                            {
                                DoctorID = i,
                                SpecialtyID = specialtyIDs[random.Next(specialtyCount)]
                            };
                            context.DoctorSpecialties.Add(ds);
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception)
                            {
                                //Go on to next in case you randomly chose the same SpecialtyID twice
                            }
                        }
                    }
                }

                //Add some Medical Trials
                if (!context.MedicalTrials.Any())
                {
                    context.MedicalTrials.AddRange(
                     new MedicalTrial
                     {
                         TrialName = "UOT - Lukemia Treatment"
                     }, new MedicalTrial
                     {
                         TrialName = "HyGIeaCare Center -  Microbiome Analysis of Constipated Versus Non-constipation Patients"
                     }, new MedicalTrial
                     {
                         TrialName = "TUK - Hair Loss Treatment"
                     });
                    context.SaveChanges();
                }


                if (!context.Patients.Any())
                {
                    context.Patients.AddRange(
                    new Patient
                    {
                        FirstName = "Fred",
                        MiddleName = "Reginald",
                        LastName = "Flintstone",
                        OHIP = "1231231234",
                        DOB = DateTime.Parse("1955-09-01"),
                        ExpYrVisits = 6,
                        Phone = 9055551212,
                        eMail = "fflintstone@outlook.com",
                        DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Gregory" && d.LastName == "House").ID,
                        MedicalTrialID = context.MedicalTrials.FirstOrDefault(d => d.TrialName.Contains("UOT")).ID
                    },
                    new Patient
                    {
                        FirstName = "Wilma",
                        MiddleName = "Jane",
                        LastName = "Flintstone",
                        OHIP = "1321321324",
                        DOB = DateTime.Parse("1964-04-23"),
                        ExpYrVisits = 2,
                        Phone = 9055551212,
                        eMail = "wflintstone@outlook.com",
                        DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Gregory" && d.LastName == "House").ID
                    },
                    new Patient
                    {
                        FirstName = "Barney",
                        LastName = "Rubble",
                        OHIP = "3213213214",
                        DOB = DateTime.Parse("1964-02-22"),
                        ExpYrVisits = 2,
                        Phone = 9055551213,
                        eMail = "brubble@outlook.com",
                        DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Doogie" && d.LastName == "Houser").ID,
                        MedicalTrialID = context.MedicalTrials.FirstOrDefault(d => d.TrialName.Contains("TUK")).ID
                    },
                    new Patient
                    {
                        FirstName = "Jane",
                        MiddleName = "Samantha",
                        LastName = "Doe",
                        OHIP = "4124124123",
                        ExpYrVisits = 2,
                        Phone = 9055551234,
                        eMail = "jdoe@outlook.com",
                        DoctorID = context.Doctors.FirstOrDefault(d => d.FirstName == "Charles" && d.LastName == "Xavier").ID
                    });
                    context.SaveChanges();
                }

                //Conditions
                if (!context.Conditions.Any())
                {
                    context.Conditions.AddRange(
                        new Condition
                        {
                            ConditionName = "Cancer"
                        },
                        new Condition
                        {
                            ConditionName = "Mumps"
                        },
                        new Condition
                        {
                            ConditionName = "Measles"
                        },
                        new Condition
                        {
                            ConditionName = "COVID-19"
                        },
                        new Condition
                        {
                            ConditionName = "Heart Attack"
                        },
                        new Condition
                        {
                            ConditionName = "Diabetes"
                        });
                    context.SaveChanges();
                }
                //PatientConditions
                if (!context.PatientConditions.Any())
                {
                    context.PatientConditions.AddRange(
                        new PatientCondition
                        {
                            ConditionID = context.Conditions.FirstOrDefault(c => c.ConditionName == "Cancer").ID,
                            PatientID = context.Patients.FirstOrDefault(p => p.LastName == "Flintstone" && p.FirstName == "Fred").ID
                        },
                        new PatientCondition
                        {
                            ConditionID = context.Conditions.FirstOrDefault(c => c.ConditionName == "Heart Attack").ID,
                            PatientID = context.Patients.FirstOrDefault(p => p.LastName == "Flintstone" && p.FirstName == "Fred").ID
                        },
                        new PatientCondition
                        {
                            ConditionID = context.Conditions.FirstOrDefault(c => c.ConditionName == "Diabetes").ID,
                            PatientID = context.Patients.FirstOrDefault(p => p.LastName == "Flintstone" && p.FirstName == "Wilma").ID
                        });
                    context.SaveChanges();
                }
                //Seed Appt Reasons
                string[] ApptReasons = new string[] { "Illness", "Accident", "Mental State", "Annual Checkup", "COVID-19" };
                if (!context.ApptReasons.Any())
                {
                    foreach (string s in ApptReasons)
                    {
                        ApptReason ar = new ApptReason
                        {
                            ReasonName = s
                        };
                        context.ApptReasons.Add(ar);
                    }
                    context.SaveChanges();
                }
                //Create 5 notes from Bacon ipsum
                string[] baconNotes = new string[] { "Bacon ipsum dolor amet meatball corned beef kevin, alcatra kielbasa biltong drumstick strip steak spare ribs swine. Pastrami shank swine leberkas bresaola, prosciutto frankfurter porchetta ham hock short ribs short loin andouille alcatra. Andouille shank meatball pig venison shankle ground round sausage kielbasa. Chicken pig meatloaf fatback leberkas venison tri-tip burgdoggen tail chuck sausage kevin shank biltong brisket.", "Sirloin shank t-bone capicola strip steak salami, hamburger kielbasa burgdoggen jerky swine andouille rump picanha. Sirloin porchetta ribeye fatback, meatball leberkas swine pancetta beef shoulder pastrami capicola salami chicken. Bacon cow corned beef pastrami venison biltong frankfurter short ribs chicken beef. Burgdoggen shank pig, ground round brisket tail beef ribs turkey spare ribs tenderloin shankle ham rump. Doner alcatra pork chop leberkas spare ribs hamburger t-bone. Boudin filet mignon bacon andouille, shankle pork t-bone landjaeger. Rump pork loin bresaola prosciutto pancetta venison, cow flank sirloin sausage.", "Porchetta pork belly swine filet mignon jowl turducken salami boudin pastrami jerky spare ribs short ribs sausage andouille. Turducken flank ribeye boudin corned beef burgdoggen. Prosciutto pancetta sirloin rump shankle ball tip filet mignon corned beef frankfurter biltong drumstick chicken swine bacon shank. Buffalo kevin andouille porchetta short ribs cow, ham hock pork belly drumstick pastrami capicola picanha venison.", "Picanha andouille salami, porchetta beef ribs t-bone drumstick. Frankfurter tail landjaeger, shank kevin pig drumstick beef bresaola cow. Corned beef pork belly tri-tip, ham drumstick hamburger swine spare ribs short loin cupim flank tongue beef filet mignon cow. Ham hock chicken turducken doner brisket. Strip steak cow beef, kielbasa leberkas swine tongue bacon burgdoggen beef ribs pork chop tenderloin.", "Kielbasa porchetta shoulder boudin, pork strip steak brisket prosciutto t-bone tail. Doner pork loin pork ribeye, drumstick brisket biltong boudin burgdoggen t-bone frankfurter. Flank burgdoggen doner, boudin porchetta andouille landjaeger ham hock capicola pork chop bacon. Landjaeger turducken ribeye leberkas pork loin corned beef. Corned beef turducken landjaeger pig bresaola t-bone bacon andouille meatball beef ribs doner. T-bone fatback cupim chuck beef ribs shank tail strip steak bacon." };
                //Create collection of the primary keys of the Reasons
                int[] ApptReasonIDs = context.ApptReasons.Select(s => s.ID).ToArray();
                int[] patientIDs = context.Patients.Select(d => d.ID).ToArray();
                //Appointments - the Intersection
                //Add a few appointments to each patient
                if (!context.Appointments.Any())
                {
                    int reasonCount = ApptReasonIDs.Count();
                    foreach (int i in patientIDs)
                    {
                        int howMany = random.Next(1, 4);
                        for (int j = 1; j <= howMany; j++)
                        {
                            Appointment a = new Appointment()
                            {
                                PatientID = i,
                                ApptReasonID = ApptReasonIDs[random.Next(reasonCount)],
                                appDate = DateTime.Today.AddDays(-1 * random.Next(120)),
                                Notes = baconNotes[random.Next(5)]
                            };
                            context.Appointments.Add(a);
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception)
                            {
                                //That one must have violated a constraint.  We will just skip it and
                                //go on to the next.
                            }
                        }
                    }
                }
            }
        }
    }
}
