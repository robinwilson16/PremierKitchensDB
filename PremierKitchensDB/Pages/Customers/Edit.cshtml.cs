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
using Newtonsoft.Json;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;
using PremierKitchensDB.Pages.Customers;
using PremierKitchensDB.Shared;

namespace PremierKitchensDB.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class EditModel : CustomerPageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public EditModel(PremierKitchensDB.Data.ApplicationDbContext context)
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
                .Include(c => c.SourceOfInformation)
                .Include(c => c.CustomerArea).ThenInclude(c => c.Area)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerID == id);

            if (Customer == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TitleID"] = new SelectList(_context.Lookup.Where(L => L.Domain == "TITLE"), "LookupCode", "LookupName");
            ViewData["ShowroomID"] = new SelectList(_context.Showroom, "ShowroomID", "ShowroomName");
            ViewData["SourceOfInformationID"] = new SelectList(_context.SourceOfInformation, "SourceOfInformationID", "SourceOfInformationName");
            
            PopulateCustomerAreaData(_context, Customer);

            await Audit.AddAuditRecord(_context, 'V', "Customer", "CustomerID", Customer.CustomerID, Identity.GetUserId(User, _context), Customer.Forename + " " + Customer.Surname + " (" + Customer.CustomerID + ") Viewed");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedAreas)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Original details
            var originalCustomer = await _context.Customer
                .Include(c => c.CustomerArea)
                    .ThenInclude(c => c.Area)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            _context.Attach(Customer).State = EntityState.Modified;

            //Override values for updated by and date
            Customer.CreatedBy = originalCustomer.CreatedBy;
            Customer.CreatedDate = originalCustomer.CreatedDate;
            Customer.UpdatedDate = DateTime.Now;
            Customer.UpdatedBy = Identity.GetUserId(User, _context);

            var customerToUpdate = await _context.Customer
                .Include(c => c.CustomerArea)
                    .ThenInclude(c => c.Area)
                .FirstOrDefaultAsync(c => c.CustomerID == id);

            try
            {
                UpdateCustomerAreas(_context, selectedAreas, customerToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerToUpdate.CustomerID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            object originalCustomerObj = new object();
            originalCustomerObj = originalCustomer;

            object newCustomerObj = new object();
            newCustomerObj = customerToUpdate;

            object originalCustomerAreasObj = new object();
            originalCustomerAreasObj = originalCustomer.CustomerArea;

            object newCustomerAreasObj = new object();
            newCustomerAreasObj = customerToUpdate.CustomerArea;

            string changes = "";

            changes = Audit.WhatChanged(originalCustomerObj, newCustomerObj, changes);

            changes = Audit.ElementsChanged(originalCustomerAreasObj, newCustomerAreasObj, "AreaID", changes);

            await Audit.AddAuditRecord(_context, 'E', "Customer", "CustomerID", Customer.CustomerID, Identity.GetUserId(User, _context), changes);
            //return RedirectToPage("./Index");
            return new JsonResult(Customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }
    }
}
