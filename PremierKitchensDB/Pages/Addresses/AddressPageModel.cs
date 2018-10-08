using Microsoft.AspNetCore.Mvc.RazorPages;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Pages.Addresses
{
    public class AddressPageModel : PageModel
    {
        public void UpdateCustomerPrimaryAddress(ApplicationDbContext context, Address updatedAddress, Customer customerToUpdate)
        {
            if (updatedAddress.IsPrimary)
            {
                foreach (var address in context.Address)
                {
                    if (address.AddressID != updatedAddress.AddressID)
                    {
                        address.IsPrimary = false;
                        context.Update(address);
                    }
                }
            }
        }
    }
}
