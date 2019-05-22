using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;
using Microsoft.Extensions.Configuration;

namespace PremierKitchensDB.Shared
{
    public class Identity
    {
        public ApplicationUser ApplicationUser { get; set; }

        public static string GetUserId(ClaimsPrincipal user, ApplicationDbContext _context)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            //In case cannot obtain current user then set to this default user as created by field is required
            if(string.IsNullOrEmpty(userId))
            {
                var defaultUser = _context.Users.FirstOrDefault(u => u.Email == "admin@rwsservices.net");
                userId = defaultUser.Id;
            }

            return userId;
        }

        public static string GetUserName(ClaimsPrincipal user, ApplicationDbContext _context)
        {
            var userName = user.Identity.Name.ToString();

            //In case cannot obtain current user then set to this default user as created by field is required
            if (string.IsNullOrEmpty(userName))
            {
                var defaultUser = _context.Users.FirstOrDefault(u => u.Email == "admin@rwsservices.net");
                userName = defaultUser.UserName;
            }

            return userName;
        }

        public static string GetGreeting()
        {
            string greeting = "";
            int currentHour = DateTime.Now.Hour;

            if (currentHour < 12)
            {
                greeting = "Good Morning";
            }
            else if (currentHour < 17)
            {
                greeting = "Good Afternoon";
            }
            else
            {
                greeting = "Good Evening";
            }

            return greeting;
        }
    }
}
