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
    public class DetailsModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public DetailsModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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

            await Shared.Audit.AddAuditRecord(_context, 'V', "Customer", "CustomerID", Customer.CustomerID, Shared.Identity.GetUserId(User, _context), Customer.Forename + " " + Customer.Surname + " (" + Customer.CustomerID + ") Viewed");

            return Page();
        }
    }
}
