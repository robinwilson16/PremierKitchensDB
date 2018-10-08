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

namespace PremierKitchensDB.Pages.Customers
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
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer = await _context.Customer
                .Include(c => c.ApplicationUserCreatedBy)
                .Include(c => c.ApplicationUserUpdatedBy)
                .Include(c => c.Showroom)
                .Include(c => c.SourceOfInformation).FirstOrDefaultAsync(m => m.CustomerID == id);

            if (Customer == null)
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

            Customer = await _context.Customer.FindAsync(id);

            if (Customer != null)
            {
                _context.Customer.Remove(Customer);
                await _context.SaveChangesAsync();

                await Shared.Audit.AddAuditRecord(_context, 'D', "Customer", "CustomerID", Customer.CustomerID, Shared.Identity.GetUserId(User, _context), "Customer Deleted: Forename: " + Customer.Forename + ", Surname: " + Customer.Surname);
            }

            //return RedirectToPage("./Index");
            return new JsonResult(Customer);
        }
    }
}
