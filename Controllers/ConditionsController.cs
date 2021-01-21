﻿using System;
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
    public class ConditionsController : Controller
    {
        private readonly MedicalOfficeContext _context;

        public ConditionsController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: Conditions
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Lookups", new { Tab = "ConditionsTab" });
        }

        // GET: Conditions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condition = await _context.Conditions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (condition == null)
            {
                return NotFound();
            }

            return View(condition);
        }

        // GET: Conditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conditions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ConditionName")] Condition condition)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(condition);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "ConditionsTab" });
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            
            return View(condition);
        }

        // GET: Conditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condition = await _context.Conditions.FindAsync(id);
            if (condition == null)
            {
                return NotFound();
            }
            return View(condition);
        }

        // POST: Conditions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var conditionToUpdate = await _context.Conditions
                .FirstOrDefaultAsync(m => m.ID == id);
            //Check that you got it or exit with a not found error
            if (conditionToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Condition>(conditionToUpdate, "",
                d => d.ConditionName))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Lookups", new { Tab = "ConditionsTab" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConditionExists(conditionToUpdate.ID))
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
            return View(conditionToUpdate);
        }

        // GET: Conditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var condition = await _context.Conditions
                .FirstOrDefaultAsync(m => m.ID == id);
            if (condition == null)
            {
                return NotFound();
            }

            return View(condition);
        }

        // POST: Conditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var condition = await _context.Conditions.FindAsync(id);
            try
            {
                _context.Conditions.Remove(condition);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Lookups", new { Tab = "ConditionsTab" });
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    ModelState.AddModelError("", "Unable to delete condition. Remember, you cannot delete a condition that any Patient has in their Medical History.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(condition);

        }

        private bool ConditionExists(int id)
        {
            return _context.Conditions.Any(e => e.ID == id);
        }
    }
}
