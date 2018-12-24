using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierKitchensDB.Pages.Customers
{
    public class Functions
    {
        public static string ToASCIIColour(string forename, string surname)
        {
            string input = "";
            if (!string.IsNullOrEmpty(forename) && !string.IsNullOrEmpty(surname))
            {
                input = forename.Substring(0, 1) + surname.Substring(0, 1);
            }
            else if (!string.IsNullOrEmpty(surname))
            {
                input = surname.Substring(0, 2);
            }
            else if (!string.IsNullOrEmpty(forename))
            {
                input = forename.Substring(0, 2);
            }
            else
            {
                input = "AA";
            }


            byte[] array = Encoding.ASCII.GetBytes(input);

            string output = "";
            string numStr;

            foreach (byte element in array)
            {
                numStr = (Int32.Parse(element.ToString()) - 36).ToString();
                output = output + numStr;
            }

            return output;

        }

        public static string GetInitials(string forename, string surname)
        {
            string input = "";
            if (!string.IsNullOrEmpty(forename) && !string.IsNullOrEmpty(surname))
            {
                input = forename.Substring(0, 1) + surname.Substring(0, 1);
            }
            else if (!string.IsNullOrEmpty(surname))
            {
                input = surname.Substring(0, 1);
            }
            else if (!string.IsNullOrEmpty(forename))
            {
                input = forename.Substring(0, 1);
            }
            else
            {
                input = "";
            }

            return input;

        }
    }
}
