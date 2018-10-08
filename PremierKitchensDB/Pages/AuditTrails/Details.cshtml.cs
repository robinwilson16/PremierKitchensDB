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
    public class DetailsModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public DetailsModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
