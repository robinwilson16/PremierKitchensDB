using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.Notes
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public CreateModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? id)
        {
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CustomerID"] = id;
            return Page();
        }

        [BindProperty]
        public Note Note { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Override values for created by and date
            Note.CreatedDate = DateTime.Now;
            Note.CreatedBy = Shared.Identity.GetUserId(User, _context);

            _context.Note.Add(Note);
            await _context.SaveChangesAsync();

            await Shared.Audit.AddAuditRecord(_context, 'C', "Note", "NoteID", Note.NoteID, Shared.Identity.GetUserId(User, _context), "Note Created");

            //return RedirectToPage("./Index");
            JObject jsonItem = JObject.FromObject(Note);

            //objectID added for form.js
            jsonItem.Add("objectID", Note.NoteID);

            return Content(jsonItem.ToString(), "application/json");
            //return new JsonResult(Note);
        }
    }
}