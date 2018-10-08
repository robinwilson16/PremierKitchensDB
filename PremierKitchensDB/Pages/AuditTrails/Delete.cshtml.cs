using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.AuditTrails
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public DeleteModel(PremierKitchensDB.Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AuditTrail = await _context.AuditTrail.FindAsync(id);

            if (AuditTrail != null)
            {
                _context.AuditTrail.Remove(AuditTrail);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
