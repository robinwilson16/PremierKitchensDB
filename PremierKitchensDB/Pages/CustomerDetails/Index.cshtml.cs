using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PremierKitchensDB.Shared;

namespace PremierKitchensDB.Pages.CustomerDetails
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(PremierKitchensDB.Data.ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CurrentSearch { get; set; }
        public string CurrentSearchSQL { get; set; }
        public string CurrentSearchHTML { get; set; }
        public string CurrentSort { get; set; }
        public string LoggedInUser { get; set; }
        public string UserGreeting { get; set; }
        public string SystemDatabase { get; set; }
        public string SystemVersion { get; set; }
        public string Browser { get; set; }

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
            LoggedInUser = Identity.GetUserName(User, _context);

            if (String.IsNullOrEmpty(sortSQL))
            {
                sort = "";
            }
            //sortSQL = "";
            //searchSQL = "";
            CurrentSort = sort;

            UserGreeting = Identity.GetGreeting();
            HttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("SystemDatabase", out string SystemDB);
            if(SystemDB != null)
            {
                SystemDatabase = SystemDB;
            }
            else
            {
                SystemDatabase = "Live";
            }

            SystemVersion = _configuration["Version"];

            Browser = Request.Headers["User-Agent"];
        }
    }
}