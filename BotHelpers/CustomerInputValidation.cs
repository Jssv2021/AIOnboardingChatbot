using System.Text.RegularExpressions;
using System;
using LaYumba.Functional;
using static LaYumba.Functional.F;
using System.Net.Http;


namespace ChatbotCustomerOnboarding.ErrorValidation
{
    public class Validator
    {
        public const string IsInvalid = "Error";
        public const string Isvalid = "";
        public const double defaultDouble = 0.0;
        public static dynamic defaultErrorResponse = null;
        public static DateTime defaultTime = default(DateTime);

        public static Func<string, bool> IsAlpha = (name) => { return Regex.Match(name, @"^[a-zA-Z ]+$").Success; };
        public static Func<string, bool> IsMobile = (mobileNumber) => { return Regex.Match(mobileNumber, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}").Success; };
        public static Func<string, bool> IsAddress = (addressLine) => { return Regex.Match(addressLine, @"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$").Success; };
        public static Func<string, bool> IsZipCode = (zipCode) => { return Regex.Match(zipCode, @"^\d{5}$").Success; };
        public static Func<string, bool> IsAphaNumeric = (text) => { return Regex.Match(text, @"^[^<>!@#%/?*]+$").Success; };


        public class Email
        {
            static readonly Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            private string Value { get; }

            private Email(string value) => Value = value;

            public static Option<Email> Create(string s)
               => regex.IsMatch(s)
                  ? Some(new Email(s))
                  : None;

            public static implicit operator string(Email e)
               => e.Value;
        }

        public class Errors
        {
            public static string Message
         => $"is Invalid Input";

        }

    }
}
