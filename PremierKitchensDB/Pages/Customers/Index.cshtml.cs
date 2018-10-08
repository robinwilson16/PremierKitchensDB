using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PremierKitchensDB.Data;
using PremierKitchensDB.Models;

namespace PremierKitchensDB.Pages.Customers
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly PremierKitchensDB.Data.ApplicationDbContext _context;

        public IndexModel(PremierKitchensDB.Data.ApplicationDbContext context)
        {
            _context = context; 
        }

        public IList<GetCustomerList> GetCustomerList { get; set; }
        public IList<CustomerArea> CustomerArea { get; set; }
        public IList<Showroom> Showroom { get; set; }

        public string CurrentSearch { get; set; }
        public string CurrentSearchSQL { get; set; }
        public string CurrentSearchHTML { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string search, string sort)
        {
            if (String.IsNullOrEmpty(search))
            {
                search = "";
            }

            if (String.IsNullOrEmpty(sort))
            {
                sort = "";
            }

            sort = DistinctOrderBy(sort);

            var searchSQL = SearchStrToSQL(search);
            var searchHtml = SearchStrToHtml(search);
            var sortSQL = OrderByStrToSQL(sort);

            CurrentSearch = search;
            CurrentSearchSQL = searchSQL;
            CurrentSearchHTML = searchHtml;

            if (String.IsNullOrEmpty(sortSQL))
            {
                sort = "";
            }
            //sortSQL = "";
            //searchSQL = "";
            CurrentSort = sort;

            var searchParam = new SqlParameter("@SearchString", searchSQL);
            var sortParam = new SqlParameter("@SortString", sortSQL);
            
            Showroom = await _context.Showroom
                .ToListAsync();

            CustomerArea = await _context.CustomerArea
                .Include(c => c.Area)
                .ToListAsync();

            GetCustomerList = await _context.GetCustomerList
                .FromSql("EXEC sp_GetCustomerList @SearchString, @SortString", searchParam, sortParam)
                .ToListAsync();

            //IQueryable<Customer> customerIQ = from c in _context.Customer
            //                                    .Include(c => c.ApplicationUserCreatedBy)
            //                                    .Include(c => c.ApplicationUserUpdatedBy)
            //                                    .Include(c => c.Showroom)
            //                                    .Include(c => c.SourceOfInformation)
            //                                  select c;

            //Customer = await _context.Customer
            //    .Include(c => c.ApplicationUserCreatedBy)
            //    .Include(c => c.ApplicationUserUpdatedBy)
            //    .Include(c => c.Showroom)
            //    .Include(c => c.SourceOfInformation).ToListAsync();

            //customerIQ = customerIQ.OrderByDescending(c => c.Surname);

            //Customer = await customerIQ.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnGetJsonAsync(string search, string sort)
        {
            //Attempt to display as JSON but cuts off /?handler=Json
            if (String.IsNullOrEmpty(search))
            {
                search = "";
            }

            if (String.IsNullOrEmpty(sort))
            {
                sort = "";
            }

            sort = DistinctOrderBy(sort);

            var searchSQL = SearchStrToSQL(search);
            var searchHtml = SearchStrToHtml(search);
            var sortSQL = OrderByStrToSQL(sort);

            CurrentSearch = search;
            CurrentSearchSQL = searchSQL;
            CurrentSearchHTML = searchHtml;

            if (String.IsNullOrEmpty(sortSQL))
            {
                sort = "";
            }
            //sortSQL = "";
            //searchSQL = "";
            CurrentSort = sort;

            var searchParam = new SqlParameter("@SearchString", searchSQL);
            var sortParam = new SqlParameter("@SortString", sortSQL);

            GetCustomerList = await _context.GetCustomerList
                .FromSql("EXEC sp_GetCustomerList @SearchString, @SortString", searchParam, sortParam)
                .ToListAsync();

            return new JsonResult(GetCustomerList);
        }

        public static string DistinctOrderBy(string str)
        {
            //Clear search string if it contains invalid text to protect the database
            var invalidInput = Regex.Replace(str, "[a-zA-Z0-9,!.]", "");
            if (invalidInput.Length > 0)
            {
                str = "";
            }

            str = str.TrimStart('!');

            //Check for and remove duplicates
            string[] fields = str.Split('!');
            List<string> fieldsList = new List<string>();
            List<string> fieldsListDist = new List<string>();
            foreach (var field in fields)
            {
                fieldsList.Add(field);
            }

            //Reverse so last sort order will remain
            fieldsList.Reverse();

            foreach (var field in fieldsList)
            {
                if(!(fieldsListDist.Contains(field) || fieldsListDist.Contains(field.Replace(",DESC", "")) || fieldsListDist.Contains(field + ",DESC")))
                {
                    fieldsListDist.Add(field);
                }
            }
            
            //Sort back to correct order with earlier dups removed
            fieldsListDist.Reverse();

            var distStr = String.Join("!", fieldsListDist.ToArray());

            return distStr;
        }

        public static string OrderByStrToSQL(string str)
        {
            str = str.TrimStart('!');
            str = str.Replace(",", " ");
            str = str.Replace("!", ", ");

            return str;
        }

        public static string SearchStrToSQL(string str)
        {
            var sqlStr = "";

            if (str.Length > 0)
            {
                str = str.TrimStart('!');

                //Turn into array to process
                string[] fields = str.Split('!');
                List<string> fieldsList = new List<string>();
                string col;
                string comp;
                string val;
                string openBracket;
                string closeBracket;

                foreach (var field in fields)
                {
                    //Check if brackets are needed
                    if (field.Contains('{'))
                    {
                        openBracket = "(";
                    }
                    else
                    {
                        openBracket = "";
                    }

                    if (field.Contains('}'))
                    {
                        closeBracket = ")";
                    }
                    else
                    {
                        closeBracket = "";
                    }

                    var fieldNoBrackets = field;
                    fieldNoBrackets = fieldNoBrackets.Replace("{", "");
                    fieldNoBrackets = fieldNoBrackets.Replace("}", "");

                    var TwoVals = Regex.Replace(fieldNoBrackets, "[{a-zA-Z0-9.~]+,[a-zA-Z0-9}]+", "");
                    var ThreeVals = Regex.Replace(fieldNoBrackets, "[{a-zA-Z0-9.~]+,[a-zA-Z0-9]+,[a-zA-Z0-9%@£.|_}]+", "");
                    bool HasTwoVals = true;
                    bool HasThreeVals = true;

                    if (TwoVals.Length > 0)
                    {
                        //Does not contain 2 values
                        HasTwoVals = false;
                    }

                    if (ThreeVals.Length > 0)
                    {
                        //Does not contain 3 values so must be a null check
                        HasThreeVals = false;
                    }

                    if (HasTwoVals)
                    {
                        col = field.Substring(0, fieldNoBrackets.IndexOf(','));
                        comp = field.Substring(fieldNoBrackets.IndexOf(',') + 1, fieldNoBrackets.Length - fieldNoBrackets.IndexOf(',') - 1);

                        //Surround value with correct chars depending on comp
                        if (comp == "NULL")
                        {
                            comp = " IS NULL";
                        }
                        else if (comp == "NULLN")
                        {
                            comp = " IS NOT NULL";
                        }
                        else
                        {
                            comp = "";
                        }

                        if (!String.IsNullOrEmpty(comp))
                        {
                            fieldsList.Add(openBracket + col + comp + closeBracket);
                        }
                    }
                    else if (HasThreeVals)
                    {
                        col = fieldNoBrackets.Substring(0, fieldNoBrackets.IndexOf(','));
                        comp = fieldNoBrackets.Substring(fieldNoBrackets.IndexOf(',') + 1, fieldNoBrackets.LastIndexOf(',') - fieldNoBrackets.IndexOf(',') - 1);
                        val = fieldNoBrackets.Substring(fieldNoBrackets.LastIndexOf(',') + 1, fieldNoBrackets.Length - fieldNoBrackets.LastIndexOf(',') - 1);
                        
                        //Replace any commas back if previously converted to |
                        val = val.Replace("|", ",");

                        //If number then remove commas and pound sign
                        val = val.Replace("£", "");
                        int valInt;
                        if (int.TryParse(val, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out valInt))
                        {
                            val = valInt.ToString();
                        }

                        //Replace any spaces back from _
                        val = val.Replace("_", " ");

                        //Surround value with correct chars depending on comp
                        if (comp == "LK")
                        {
                            //If comparison is LIKE ensure value contains % and if not surround with %
                            if (!val.Contains("%"))
                            {
                                val = "'%" + val + "%'";
                            }
                            else
                            {
                                val = "'" + val + "'";
                            }
                        }
                        else if (comp == "LKB")
                        {
                            val = "'%" + val + "'";
                        }
                        else if (comp == "LKE")
                        {
                            val = "'" + val + "%'";
                        }
                        else
                        {
                            if (!Regex.IsMatch(val, @"^\d+$"))
                            {
                                val = "'" + val + "'";
                            }
                        }

                        //Replace comparison symbols
                        comp = comp.Replace("NEQ", " <> ");
                        comp = comp.Replace("EQ", " = ");
                        comp = comp.Replace("GT", " > ");
                        comp = comp.Replace("LT", " < ");
                        comp = comp.Replace("LKB", " LIKE ");
                        comp = comp.Replace("LKE", " LIKE ");
                        comp = comp.Replace("LK", " LIKE ");

                        fieldsList.Add(openBracket + col + comp + val + closeBracket);
                    }
                }

                sqlStr = String.Join(" AND ", fieldsList.ToArray());

                //Replace ANDs with ORs
                sqlStr = sqlStr.Replace("AND ~", "OR ");
                sqlStr = sqlStr.Replace("AND (~", "OR (");

                //Lastly check if brackets are left open and close
                int OpenBracketCount = sqlStr.Length - sqlStr.Replace("(", "").Length;
                int CloseBracketCount = sqlStr.Length - sqlStr.Replace(")", "").Length;

                if (OpenBracketCount > CloseBracketCount)
                {
                    for (int i = CloseBracketCount; i < OpenBracketCount; i++)
                    {
                        sqlStr = sqlStr + ")";
                    }
                }
            }

            return sqlStr;
        }

        public static string SearchStrToHtml(string str)
        {
            var htmlStr = "";

            if (str.Length > 0)
            {
                str = str.TrimStart('!');

                //Turn into array to process
                string[] fields = str.Split('!');
                List<string> fieldsList = new List<string>();
                string col;
                string comp;
                string val;
                string openBracket;
                string closeBracket;

                foreach (var field in fields)
                {
                    //Check if brackets are needed
                    if (field.Contains('{'))
                    {
                        openBracket = "(";
                    }
                    else
                    {
                        openBracket = "";
                    }

                    if (field.Contains('}'))
                    {
                        closeBracket = ")";
                    }
                    else
                    {
                        closeBracket = "";
                    }

                    var fieldNoBrackets = field;
                    fieldNoBrackets = fieldNoBrackets.Replace("{", "");
                    fieldNoBrackets = fieldNoBrackets.Replace("}", "");

                    var TwoVals = Regex.Replace(fieldNoBrackets, "[{a-zA-Z0-9.~]+,[a-zA-Z0-9}]+", "");
                    var ThreeVals = Regex.Replace(fieldNoBrackets, "[{a-zA-Z0-9.~]+,[a-zA-Z0-9]+,[a-zA-Z0-9%@£.|_}]+", "");
                    bool HasTwoVals = true;
                    bool HasThreeVals = true;

                    if (TwoVals.Length > 0)
                    {
                        //Does not contain 2 values
                        HasTwoVals = false;
                    }

                    if (ThreeVals.Length > 0)
                    {
                        //Does not contain 3 values so must be a null check
                        HasThreeVals = false;
                    }

                    if (HasTwoVals)
                    {
                        col = field.Substring(0, fieldNoBrackets.IndexOf(','));
                        comp = field.Substring(fieldNoBrackets.IndexOf(',') + 1, fieldNoBrackets.Length - fieldNoBrackets.IndexOf(',') - 1);

                        //Remove table alias from col name
                        if (col.IndexOf('.') > 0)
                        {
                            col = col.Substring(col.IndexOf('.') + 1, col.Length - col.IndexOf('.') - 1);
                        }

                        //Surround value with correct chars depending on comp
                        if (comp == "NULL")
                        {
                            comp = " has no value";
                        }
                        else if (comp == "NULLN")
                        {
                            comp = " has a value";
                        }
                        else
                        {
                            comp = "";
                        }

                        if (!String.IsNullOrEmpty(comp))
                        {
                            fieldsList.Add(openBracket + col + comp + closeBracket);
                        }
                    }
                    else if (HasThreeVals)
                    {
                        col = fieldNoBrackets.Substring(0, fieldNoBrackets.IndexOf(','));
                        comp = fieldNoBrackets.Substring(fieldNoBrackets.IndexOf(',') + 1, fieldNoBrackets.LastIndexOf(',') - fieldNoBrackets.IndexOf(',') - 1);
                        val = fieldNoBrackets.Substring(fieldNoBrackets.LastIndexOf(',') + 1, fieldNoBrackets.Length - fieldNoBrackets.LastIndexOf(',') - 1);

                        //Replace any commas back if previously converted to |
                        val = val.Replace("|", ",");

                        //Replace any spaces back from _
                        val = val.Replace("_", " ");

                        //Remove table alias from col name
                        if (col.IndexOf('.') > 0)
                        {
                            col = col.Substring(col.IndexOf('.') + 1, col.Length - col.IndexOf('.') - 1);
                        }

                        //Surround value with correct chars depending on comp
                        if (comp == "LK")
                        {
                            val = " contains '" + val + "'";
                        }
                        else if (comp == "LKB")
                        {
                            val = " starts with '" + val + "'";
                        }
                        else if (comp == "LKE")
                        {
                            val = " end with '" + val + "'";
                        }
                        else
                        {
                            val = " is '" + val + "'";
                        }

                        fieldsList.Add(openBracket + col + val + closeBracket);
                    }
                }

                htmlStr = String.Join("</li><li>And ", fieldsList.ToArray());

                //Replace ANDs with ORs
                htmlStr = htmlStr.Replace("And ~", "Or ");
                htmlStr = htmlStr.Replace("And (~", "Or (");

                //Lastly check if brackets are left open and close
                int OpenBracketCount = htmlStr.Length - htmlStr.Replace("(", "").Length;
                int CloseBracketCount = htmlStr.Length - htmlStr.Replace(")", "").Length;

                if (OpenBracketCount > CloseBracketCount)
                {
                    for (int i = CloseBracketCount; i < OpenBracketCount; i++)
                    {
                        htmlStr = htmlStr + ")";
                    }
                }
            }

            htmlStr = "<ul><li>" + htmlStr + "</li></ul>";

            return htmlStr;
        }
    }
}
