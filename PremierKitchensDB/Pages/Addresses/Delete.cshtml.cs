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

namespace PremierKitchensDB.Pages.Addresses
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
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _context.Address
                .Include(a => a.AddressType)
                .Include(a => a.ApplicationUserCreatedBy)
                .Include(a => a.ApplicationUserUpdatedBy)
                .Include(a => a.Customer).FirstOrDefaultAsync(m => m.AddressID == id);

            if (Address == null)
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

            Address = await _context.Address.FindAsync(id);

            if (Address != null)
            {
                _context.Address.Remove(Address);
                await _context.SaveChangesAsync();

                await Shared.Audit.AddAuditRecord(_context, 'D', "Address", "AddressID", Address.AddressID, Shared.Identity.GetUserId(User, _context), "Address Deleted: Address1: " + Address.Address1 + ", Post Code: " + Address.PostcodeOut + " " + Address.PostcodeIn);
            }

            //return RedirectToPage("./Index");
            return new JsonResult(Address);
        }
    }
}
