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
    public class IndexModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public IndexModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Address> Address { get;set; }

        public async Task OnGetAsync(int? id)
        {
            IQueryable<Address> addressIQ = from a in _context.Address
                                            .Include(a => a.AddressType)
                                            .Include(a => a.ApplicationUserCreatedBy)
                                            .Include(a => a.ApplicationUserUpdatedBy)
                                            .Include(a => a.Customer)
                                            select a;

            //Address = await _context.Address
            //    .Include(a => a.AddressType)
            //    .Include(a => a.ApplicationUserCreatedBy)
            //    .Include(a => a.ApplicationUserUpdatedBy)
            //    .Include(a => a.Customer).ToListAsync();

            if(id > 0)
            {
                addressIQ = addressIQ.Where(a => a.CustomerID == id);
            }
            else
            {
                //If no customer specified then avoid showing all addresses (i.e. for new customer screen)
                addressIQ = addressIQ.Where(a => a.CustomerID == 0);
            }

            Address = await addressIQ.AsNoTracking().ToListAsync();
        }
    }
}
