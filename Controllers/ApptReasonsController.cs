using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicalOffice.Data;
using MedicalOffice.Models;
using Microsoft.AspNetCore.Authorization;

namespace MedicalOffice.Controllers
{
    [Authorize]
    public class ApptReasonsController : Controller
    {
        private readonly MedicalOfficeContext _context;

        public ApptReasonsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: ApptReasons
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups", new { Tab = "ApptReasonsTab" });
        }

        // GET: ApptReasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apptReason = await _context.ApptReasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (apptReason == null)
            {
                return NotFound();
            }

            return View(apptReason);
        }

        // GET: ApptReasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ApptReasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ReasonName")] ApptReason apptReason)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(apptReason);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "ApptReasonsTab" });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(apptReason);
        }

        // GET: ApptReasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apptReason = await _context.ApptReasons.FindAsync(id);
            if (apptReason == null)
            {
                return NotFound();
            }
            return View(apptReason);
        }

        // POST: ApptReasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var apptReasonToUpdate = await _context.ApptReasons
                .FirstOrDefaultAsync(m => m.ID == id);
            //Check that you got it or exit with a not found error
            if (apptReasonToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<ApptReason>(apptReasonToUpdate, "",
                d => d.ReasonName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "ApptReasonsTab" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApptReasonExists(apptReasonToUpdate.ID))
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
            return View(apptReasonToUpdate);
        }

        // GET: ApptReasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apptReason = await _context.ApptReasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (apptReason == null)
            {
                return NotFound();
            }

            return View(apptReason);
        }

        // POST: ApptReasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apptReason = await _context.ApptReasons.FindAsync(id);
            try
            {
                _context.ApptReasons.Remove(apptReason);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups", new { Tab = "ApptReasonsTab" });
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to delete Reason for Appointment. Remember, you cannot delete a reason that any Patient has had for an apointment.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(apptReason);
        }

        private bool ApptReasonExists(int id)
        {
            return _context.ApptReasons.Any(e => e.ID == id);
        }
    }
}
