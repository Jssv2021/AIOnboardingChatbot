using System.Text.RegularExpressions;
using System;

namespace ChatbotCustomerOnboarding.ErrorValidation
{
    public class InputValidation
    {
        public static Func<string, bool> FirstName = (customerInfo) => { return !Regex.Match(customerInfo, "^[A-Z][a-zA-Z]{1,50}$").Success; };
        public static Func<string, bool> Mobile = (customerInfo) => { return !Regex.Match(customerInfo, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").Success; };
        public static Func<string, bool> DateOfBirth = (customerInfo) => { return !Regex.Match(customerInfo, @"^(0[1-9]|1[0-9]|2[0-9]|3[0,1])([/+-])(0[1-9]|1[0-2])([/+-])(19|20)[0-9]{2}$").Success; };
        public static Func<string, bool> AddressLine1 = (customerInfo) => { return !Regex.Match(customerInfo, @"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success; };
        public static Func<string, bool> ZipCode = (customerInfo) => { return !Regex.Match(customerInfo, @"^\d{5}$").Success; };
        public static Func<string, bool> State = (customerInfo) => { return !Regex.Match(customerInfo, @"^([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success; };
        public static Func<string, bool> Email = (customerInfo) => { return !Regex.Match(customerInfo, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success; };

    }

}
