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

namespace PremierKitchensDB.Pages.Notes
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Note = await _context.Note.FindAsync(id);

            if (Note != null)
            {
                _context.Note.Remove(Note);
                await _context.SaveChangesAsync();

                await Shared.Audit.AddAuditRecord(_context, 'D', "Note", "NoteID", Note.NoteID, Shared.Identity.GetUserId(User, _context), "Note Deleted: Note: " + Note.NoteText);
            }

            //return RedirectToPage("./Index");
            return new JsonResult(Note);
        }
    }
}
