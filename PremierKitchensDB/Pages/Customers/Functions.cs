using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PremierKitchensDB.Pages.Customers
{
    public class Functions
    {
        public static string ToASCIIColour(string input)
        {
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
    }
}
