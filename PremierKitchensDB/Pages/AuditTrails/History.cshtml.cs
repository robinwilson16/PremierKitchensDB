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

namespace PremierKitchensDB.Pages.AuditTrails
{
    [Authorize(Roles = "Admin")]
    public class HistoryModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public HistoryModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<AuditTrail> AuditTrail { get;set; }

        public async Task OnGetAsync(string tableName, string user)
        {
            AuditTrail = await _context.AuditTrail
                .FromSqlInterpolated($"EXEC sp_GetUserHistory @UserName={user}, @TableName={tableName}")
                .ToListAsync();
        }

        public async Task<IActionResult> OnGetJsonAsync(string tableName, string user)
        {
            AuditTrail = await _context.AuditTrail
                .FromSqlInterpolated($"EXEC sp_GetUserHistory @UserName={user}, @TableName={tableName}")
                .ToListAsync();

            var collectionWrapper = new
            {
                History = AuditTrail
            };

            return new JsonResult(AuditTrail);
        }
    }
}
