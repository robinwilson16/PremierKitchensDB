using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PremierKitchensDB.Pages.CustomerDetails
{
    [Authorize(Roles = "Admin")]
    public class IndexOldModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public IndexOldModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public string CurrentSearch { get; set; }
        public string CurrentSearchSQL { get; set; }
        public string CurrentSearchHTML { get; set; }
        public string CurrentSort { get; set; }
        public string LoggedInUser { get; set; }

        public void OnGet(string search, string sort)
        {
            if (String.IsNullOrEmpty(search))
            {
                search = "";
            }

            if (String.IsNullOrEmpty(sort))
            {
                sort = "";
            }

            sort = Customers.IndexModel.DistinctOrderBy(sort);

            var searchSQL = Customers.IndexModel.SearchStrToSQL(search);
            var searchHtml = Customers.IndexModel.SearchStrToHtml(search);
            var sortSQL = Customers.IndexModel.OrderByStrToSQL(sort);

            CurrentSearch = search;
            CurrentSearchSQL = searchSQL;
            CurrentSearchHTML = searchHtml;
            LoggedInUser = Shared.Identity.GetUserName(User, _context);

            if (String.IsNullOrEmpty(sortSQL))
            {
                sort = "";
            }
            //sortSQL = "";
            //searchSQL = "";
            CurrentSort = sort;
        }
    }
}