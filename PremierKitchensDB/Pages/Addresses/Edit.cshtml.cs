using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.Addresses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : AddressPageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public EditModel(PremierKitchensDB.Data.ApplicationDbContext context)
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
           ViewData["AddressTypeID"] = new SelectList(_context.AddressType, "AddressTypeID", "AddressTypeName");
           ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["CustomerID"] = new SelectList(_context.Customer, "CustomerID", "Forename");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Original details
            var originalAddress = await _context.Address
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.AddressID == Address.AddressID);

            _context.Attach(Address).State = EntityState.Modified;

            //Override values for updated by and date
            Address.UpdatedDate = DateTime.Now;
            Address.UpdatedBy = Shared.Identity.GetUserId(User, _context);

            var customerToUpdate = await _context.Customer
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.CustomerID == Address.CustomerID);

            try
            {
                UpdateCustomerPrimaryAddress(_context, Address, customerToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(Address.AddressID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            object originalObj = new object();
            originalObj = originalAddress;

            object newObj = new object();
            newObj = Address;

            string changes = Shared.Audit.WhatChanged(originalObj, newObj, "");

            await Shared.Audit.AddAuditRecord(_context, 'E', "Address", "AddressID", Address.AddressID, Shared.Identity.GetUserId(User, _context), changes);

            //return RedirectToPage("./Index");
            return new JsonResult(Address);
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressID == id);
        }
    }
}
