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
using PremierKitchensDB.Shared;

namespace PremierKitchensDB.Pages.Addresses
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
            ViewData["AddressTypeID"] = new SelectList(_context.AddressType, "AddressTypeID", "AddressTypeName");
            ViewData["CreatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UpdatedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CustomerID"] = id;
            return Page();
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Override values for created by and date
            Address.CreatedDate = DateTime.Now;
            Address.CreatedBy = Identity.GetUserId(User, _context);

            _context.Address.Add(Address);
            await _context.SaveChangesAsync();

            await Audit.AddAuditRecord(_context, 'C', "Address", "AddressID", Address.AddressID, Identity.GetUserId(User, _context), "Address Created");

            //return RedirectToPage("./Index");
            JObject jsonItem = JObject.FromObject(Address);

            //objectID added for form.js
            jsonItem.Add("objectID", Address.AddressID);

            return Content(jsonItem.ToString(), "application/json");
            //return new JsonResult(Address);
        }
    }
}