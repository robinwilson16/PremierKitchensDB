using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.Notes
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public EditModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Note Note { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Note = await _context.Note
                .Include(n => n.ApplicationUserCreatedBy)
                .Include(n => n.ApplicationUserUpdatedBy)
                .Include(n => n.Customer).FirstOrDefaultAsync(m => m.NoteID == id);

            if (Note == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Forename");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Original details
            var originalNote = await _context.Note
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NoteID == Note.NoteID);

            _context.Attach(Note).State = EntityState.Modified;

            //Override values for updated by and date
            Note.UpdatedDate = DateTime.Now;
            Note.UpdatedBy = Shared.Identity.GetUserId(User, _context);

            var customerToUpdate = await _context.Customer
                .Include(c => c.Note)
                .FirstOrDefaultAsync(c => c.CustomerID == Note.CustomerID);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(Note.NoteID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            object originalObj = new object();
            originalObj = originalNote;

            object newObj = new object();
            newObj = Note;

            string changes = Shared.Audit.WhatChanged(originalObj, newObj, "");

            await Shared.Audit.AddAuditRecord(_context, 'E', "Note", "NoteID", Note.NoteID, Shared.Identity.GetUserId(User, _context), changes);

            //return RedirectToPage("./Index");
            return new JsonResult(Note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.NoteID == id);
        }
    }
}
