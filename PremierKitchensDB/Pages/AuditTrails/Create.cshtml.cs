using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.AuditTrails
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public CreateModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public AuditTrail AuditTrail { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AuditTrail.Add(AuditTrail);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}