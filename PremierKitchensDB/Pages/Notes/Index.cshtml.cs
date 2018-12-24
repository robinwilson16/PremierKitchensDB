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
    public class IndexModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public IndexModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Note> Note { get;set; }

        public async Task OnGetAsync(int? id)
        {
            IQueryable<Note> noteIQ = from n in _context.Note
                                                .Include(n => n.ApplicationUserCreatedBy)
                                                .Include(n => n.ApplicationUserUpdatedBy)
                                                .Include(n => n.Customer)
                                                .OrderByDescending(n => n.CreatedDate)

                                      select n;

            //Note = await _context.Note
            //    .Include(n => n.ApplicationUserCreatedBy)
            //    .Include(n => n.ApplicationUserUpdatedBy)
            //    .Include(n => n.Customer).ToListAsync();

            if (id > 0)
            {
                noteIQ = noteIQ.Where(n => n.CustomerID == id);
            }
            else
            {
                //If no customer specified then avoid showing all notes (i.e. for new customer screen)
                noteIQ = noteIQ.Where(n => n.CustomerID == 0);
            }

            Note = await noteIQ.AsNoTracking().ToListAsync();

            ViewData["Alerts"] = GetAlerts(Note);
        }

        public async Task<IActionResult> OnGetJsonAsync(int? id, bool? alertOnly)
        {
            IQueryable<Note> noteIQ = from n in _context.Note
                                                .Include(n => n.ApplicationUserCreatedBy)
                                                .Include(n => n.ApplicationUserUpdatedBy)
                                                .Include(n => n.Customer)
                                                .OrderByDescending(n => n.CreatedDate)
                                      select n;

            if (id > 0)
            {
                noteIQ = noteIQ.Where(n => n.CustomerID == id);
            }
            else
            {
                //If no customer specified then avoid showing all notes (i.e. for new customer screen)
                noteIQ = noteIQ.Where(n => n.CustomerID == 0);
            }

            if (alertOnly == true)
            {
                noteIQ = noteIQ.Where(n => n.IsAlert == true);
            }

            Note = await noteIQ.AsNoTracking().ToListAsync();

            var collectionWrapper = new
            {
                Notes = Note
            };

            return new JsonResult(Note);
        }

        public static string GetAlerts(IList<Note> notes)
        {
            string alerts = "";
            foreach (var note in notes)
            {
                if(note.IsAlert)
                {
                    if(alerts == "")
                    {
                        alerts += note.NoteText;
                    }
                    else
                    {
                        alerts += "|" + note.NoteText;
                    }
                }
            }

            return alerts;
        }
    }
}
