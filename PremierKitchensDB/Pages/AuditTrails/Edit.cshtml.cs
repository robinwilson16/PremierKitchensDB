using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.AuditTrails
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
        public AuditTrail AuditTrail { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AuditTrail = await _context.AuditTrail
                .Include(a => a.ApplicationUserUpdatedBy).FirstOrDefaultAsync(m => m.AuditTrailID == id);

            if (AuditTrail == null)
            {
                return NotFound();
            }
           ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AuditTrail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditTrailExists(AuditTrail.AuditTrailID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AuditTrailExists(int id)
        {
            return _context.AuditTrail.Any(e => e.AuditTrailID == id);
        }
    }
}
