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
using PremierKitchensDB.Pages.Customers;

namespace PremierKitchensDB.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : CustomerPageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public CreateModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["TitleID"] = new SelectList(_context.Lookup.Where(L => L.Domain == "TITLE"), "LookupCode", "LookupName");
            ViewData["ShowroomID"] = new SelectList(_context.Showroom, "ShowroomID", "ShowroomName");
            ViewData["SourceOfInformationID"] = new SelectList(_context.SourceOfInformation, "SourceOfInformationID", "SourceOfInformationName");
            ViewData["DateOfEnquiry"] = DateTime.Today.ToString("yyyy-MM-dd");

            var customer = new Customer();
            customer.CustomerArea = new List<CustomerArea>();

            PopulateCustomerAreaData(_context, customer);
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public async Task<IActionResult> OnPostAsync(string[] selectedAreas)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Override values for created by and date
            Customer.CreatedDate = DateTime.Now;
            Customer.CreatedBy = Shared.Identity.GetUserId(User, _context);

            if (selectedAreas != null)
            {
                Customer.CustomerArea = new List<CustomerArea>();
                foreach (var area in selectedAreas)
                {
                    var areaToAdd = new CustomerArea
                    {
                        AreaID = int.Parse(area)
                    };
                    Customer.CustomerArea.Add(areaToAdd);

                }
            }

            _context.Customer.Add(Customer);
            await _context.SaveChangesAsync();

            await Shared.Audit.AddAuditRecord(_context, 'C', "Customer", "CustomerID", Customer.CustomerID, Shared.Identity.GetUserId(User, _context), "Customer Created");

            //return RedirectToPage("./Index");
            JObject jsonItem = JObject.FromObject(Customer);

            //objectID added for form.js
            jsonItem.Add("objectID", Customer.CustomerID);

            return Content(jsonItem.ToString(), "application/json");
            //return new JsonResult(Customer);
        }
    }
}