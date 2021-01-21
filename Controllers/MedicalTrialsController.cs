using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedicalOffice.Data;
using MedicalOffice.Models;
using MedicalOffice.Utilities;
using MedicalOffice.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MedicalOffice.Controllers
{
    [Authorize]
    public class MedicalTrialsController : Controller
    {
        //for sending email
        private readonly IMyEmailSender _emailSender;
        private readonly MedicalOfficeContext _context;

        public MedicalTrialsController(MedicalOfficeContext context, IMyEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: MedicalTrials
        public async Task<IActionResult> Index()
        {
            return View(await _context.MedicalTrials.OrderBy(m=>m.TrialName).ToListAsync());
        }

        // GET/POST: MedicalTrials/Notification/5
        public async Task<IActionResult> Notification(int? id, string Subject, string emailContent, string TrialName)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["id"] = id;
            ViewData["TrialName"] = TrialName;

            if (string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(emailContent))
            {
                ViewData["Message"] = "You must enter both a Subject and some message Content before sending the message.";
            }
            else
            {
                int folksCount = 0;
                try
                {
                    //Send a Notice.
                    List<EmailAddress> folks = (from p in _context.Patients
                                                where p.MedicalTrialID == id
                                                select new EmailAddress
                                                {
                                                    Name = p.FullName,
                                                    Address = p.eMail
                                                }).ToList();
                    folksCount = folks.Count();
                    if (folksCount > 0)
                    {
                        var msg = new EmailMessage()
                        {
                            ToAddresses = folks,
                            Subject = Subject,
                            Content = "<p>" + emailContent + "</p><p>Please access the <strong>Niagara College</strong> web site to review.</p>"

                        };
                        await _emailSender.SendToManyAsync(msg);
                        ViewData["Message"] = "Message sent to " + folksCount + " Patient"
                            + ((folksCount == 1) ? "." : "s.");
                    }
                    else
                    {
                        ViewData["Message"] = "Message NOT sent!  No Patients in medical trial.";
                    }
                }
                catch (Exception ex)
                {
                    string errMsg = ex.GetBaseException().Message;
                    ViewData["Message"] = "Error: Could not send email message to the " + folksCount + " Patient"
                        + ((folksCount == 1) ? "" : "s") + " in the trial.";
                }
            }
            return View();
        }

        // GET: MedicalTrials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalTrial = await _context.MedicalTrials
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicalTrial == null)
            {
                return NotFound();
            }

            return View(medicalTrial);
        }

        // GET: MedicalTrials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MedicalTrials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TrialName")] MedicalTrial medicalTrial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicalTrial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicalTrial);
        }

        // GET: MedicalTrials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalTrial = await _context.MedicalTrials.FindAsync(id);
            if (medicalTrial == null)
            {
                return NotFound();
            }
            return View(medicalTrial);
        }

        // POST: MedicalTrials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TrialName")] MedicalTrial medicalTrial)
        {
            if (id != medicalTrial.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicalTrial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicalTrialExists(medicalTrial.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(medicalTrial);
        }

        // GET: MedicalTrials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicalTrial = await _context.MedicalTrials
                .FirstOrDefaultAsync(m => m.ID == id);
            if (medicalTrial == null)
            {
                return NotFound();
            }

            return View(medicalTrial);
        }

        // POST: MedicalTrials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalTrial = await _context.MedicalTrials.FindAsync(id);
            _context.MedicalTrials.Remove(medicalTrial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicalTrialExists(int id)
        {
            return _context.MedicalTrials.Any(e => e.ID == id);
        }
    }
}
