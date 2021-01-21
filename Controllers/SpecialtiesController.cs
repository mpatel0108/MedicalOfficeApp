﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicalOffice.Data;
using MedicalOffice.Models;
using MedicalOffice.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace MedicalOffice.Controllers
{
    [Authorize]
    public class SpecialtiesController : Controller
    {
        private readonly MedicalOfficeContext _context;

        public SpecialtiesController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: Specialties
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups", new { Tab = "SpecialtiesTab" });
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .FirstOrDefaultAsync(m => m.ID == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // GET: Specialties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specialties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SpecialtyName")] Specialty specialty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(specialty);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "SpecialtiesTab" });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(specialty);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Specialties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var specialtyToUpdate = await _context.Specialties
                .FirstOrDefaultAsync(m => m.ID == id);
            //Check that you got it or exit with a not found error
            if (specialtyToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Specialty>(specialtyToUpdate, "",
                d => d.SpecialtyName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "SpecialtiesTab" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialtyToUpdate.ID))
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
            return View(specialtyToUpdate);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialties
                .FirstOrDefaultAsync(m => m.ID == id);
            if (specialty == null)
            {
                return NotFound();
            }

            return View(specialty);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialty = await _context.Specialties.FindAsync(id);
            try
            {
                _context.Specialties.Remove(specialty);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups", new { Tab = "SpecialtiesTab" });
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to delete specialty. Remember, you cannot delete a specialty that any Doctor has.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(specialty);

        }

        private bool SpecialtyExists(int id)
        {
            return _context.Specialties.Any(e => e.ID == id);
        }
    }
}
