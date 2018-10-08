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
    public class DetailsModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public DetailsModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
