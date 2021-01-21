using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicalOffice.Data;
using MedicalOffice.Models;
using MedicalOffice.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace MedicalOffice.Controllers
{
    [Authorize]
    public class DoctorsController : Controller
    {
        private readonly MedicalOfficeContext _context;

        public DoctorsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var doctors = from d in _context.Doctors
                .Include(d => d.DoctorDocuments)
                .Include(d => d.DoctorSpecialties)
                .ThenInclude(d => d.Specialty)
                          select d;
            return View(await doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            Doctor doctor = new Doctor();
            PopulateAssignedSpecialtyData(doctor);
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName")] Doctor doctor,
            string[] selectedOptions, List<IFormFile> theFiles)
        {
            try
            {
                UpdateDoctorSpecialties(selectedOptions, doctor);
                if (ModelState.IsValid)
                {
                    await AddDocumentsAsync(doctor, theFiles);
                    _context.Add(doctor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            //Validation Error so give the user another chance.
            PopulateAssignedSpecialtyData(doctor);
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
               .Include(d => d.DoctorDocuments)
               .Include(d => d.DoctorSpecialties)
               .ThenInclude(d => d.Specialty)
               .AsNoTracking()
               .SingleOrDefaultAsync(d => d.ID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            PopulateAssignedSpecialtyData(doctor);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            string[] selectedOptions, List<IFormFile> theFiles)
        {
            //Go get the Doctor to update
            var doctorToUpdate = await _context.Doctors
                .Include(d => d.DoctorDocuments)
                .Include(d => d.DoctorSpecialties)
                .ThenInclude(d => d.Specialty)
                .SingleOrDefaultAsync(p => p.ID == id);

            //Check that you got it or exit with a not found error
            if (doctorToUpdate == null)
            {
                return NotFound();
            }

            //Update the Doctor's Specialties
            UpdateDoctorSpecialties(selectedOptions, doctorToUpdate);

            //Try updating it with the values posted
            if (await TryUpdateModelAsync<Doctor>(doctorToUpdate, "",
                d => d.FirstName, d => d.MiddleName, d => d.LastName))
            {
                try
                {
                    await AddDocumentsAsync(doctorToUpdate, theFiles);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctorToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
                
            }

            //Validation Error so give the user another chance.
            PopulateAssignedSpecialtyData(doctorToUpdate);
            return View(doctorToUpdate);
        }

        // GET: Doctors/Delete/5
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.ID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            try
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to Delete Doctor. Remember, you cannot delete a Doctor that has patients assigned.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(doctor);
        }

        public FileContentResult Download(int id)
        {
            var theFile = _context.DoctorDocuments
                .Include(d => d.FileContent)
                .Where(f => f.ID == id)
                .SingleOrDefault();
            return File(theFile.FileContent.Content, theFile.MimeType, theFile.FileName);
        }

        public PartialViewResult ListOfDoctorDocumentDetails(int id)
        {
            var query = from d in _context.DoctorDocuments
                        where d.DoctorID == id
                        orderby d.FileName
                        select d;
            return PartialView("_ListOfDoctorDocuments", query.ToList());
        }

        public PartialViewResult ListOfSpecialtiesDetails(int id)
        {
            var query = from s in _context.DoctorSpecialties.Include(p => p.Specialty)
                        where s.DoctorID == id
                        orderby s.Specialty.SpecialtyName
                        select s;
            return PartialView("_ListOfSpecialities", query.ToList());
        }

        public PartialViewResult ListOfPatientsDetails(int id)
        {
            var query = from p in _context.Patients
                        where p.DoctorID == id
                        orderby p.LastName, p.FirstName
                        select p;
            return PartialView("_ListOfPatients", query.ToList());
        }

        private async Task AddDocumentsAsync(Doctor doctor, List<IFormFile> theFiles)
        {
            foreach (var f in theFiles)
            {
                if (f != null)
                {
                    string mimeType = f.ContentType;
                    string fileName = Path.GetFileName(f.FileName);
                    long fileLength = f.Length;
                    //Note: you could filter for mime types if you only want to allow
                    //certain types of files.  I am allowing everything.
                    if (!(fileName == "" || fileLength == 0))//Looks like we have a file!!!
                    {
                        DoctorDocument d = new DoctorDocument();
                        using (var memoryStream = new MemoryStream())
                        {
                            await f.CopyToAsync(memoryStream);
                            d.FileContent.Content = memoryStream.ToArray();
                        }
                        d.MimeType = mimeType;
                        d.FileName = fileName;
                        doctor.DoctorDocuments.Add(d);
                    };
                }
            }
        }

        private void PopulateAssignedSpecialtyData(Doctor doctor)
        {
            var allOptions = _context.Specialties;
            var currentOptionsHS = new HashSet<int>(doctor.DoctorSpecialties.Select(b => b.SpecialtyID));
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var s in allOptions)
            {
                if (currentOptionsHS.Contains(s.ID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.SpecialtyName
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = s.ID,
                        DisplayText = s.SpecialtyName
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(s => s.DisplayText), "ID", "DisplayText");
            ViewData["availOpts"] = new MultiSelectList(available.OrderBy(s => s.DisplayText), "ID", "DisplayText");
        }
        private void UpdateDoctorSpecialties(string[] selectedOptions, Doctor doctorToUpdate)
        {
            if (selectedOptions == null)
            {
                doctorToUpdate.DoctorSpecialties = new List<DoctorSpecialty>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(doctorToUpdate.DoctorSpecialties.Select(b => b.SpecialtyID));
            foreach (var s in _context.Specialties)
            {
                if (selectedOptionsHS.Contains(s.ID.ToString()))
                {
                    if (!currentOptionsHS.Contains(s.ID))
                    {
                        doctorToUpdate.DoctorSpecialties.Add(new DoctorSpecialty
                        {
                            SpecialtyID = s.ID,
                            DoctorID = doctorToUpdate.ID
                        });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(s.ID))
                    {
                        DoctorSpecialty specToRemove = doctorToUpdate.DoctorSpecialties.SingleOrDefault(d => d.SpecialtyID == s.ID);
                        _context.Remove(specToRemove);
                    }
                }
            }
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
