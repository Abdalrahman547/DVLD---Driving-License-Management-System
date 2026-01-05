using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DVLD___Driving_License_Management
{
    public class clsValidation
    {

        public static bool ValidateEmail(string Email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            var regex = new Regex(pattern);

            return regex.IsMatch(Email);
        }

        public static bool ValidateInteger(string input)
        {
            var pattern = @"^-?\d+$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public static bool ValidateFloat(string input)
        {
            var pattern = @"^-?\d+(\.\d+)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

        public static bool IsNumber(string input)
        {
            var pattern = @"^-?\d+(\.\d+)?$";
            var regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}
