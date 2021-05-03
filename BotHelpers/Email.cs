using LaYumba.Functional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatbotCustomerOnboarding.BotHelpers
{
    using static F;

    public class Email
    {
        private string _emailAddress { get; }
        // Private field to store emailAddress after its value is validated
        private Email(string emailAddress) => _emailAddress = emailAddress;
        /// <summary>
        /// Validates the emailAddress input and returns an Option value
        /// depending on whether or not the email address is valid.
        /// </summary>
        /// <param name="emailAddress">String representing the email address that we want to check.</param>
        /// <returns>Option value, None if invalid and Some(emailAddress) if valid.</returns>
        public static Option<Email> Validate(string emailAddress) =>
            IsValid(emailAddress) ? Some(new Email(emailAddress)) : None;
        /// <summary>
        /// Runs the email address through a regex to check for "@" and "." in appropriate
        /// locations in the string (i.e. "@" can't be the first character, "." can't be the 
        /// last character, "@" must come before "."
        /// </summary>
        /// <param name="emailAddress">String representing the email address that we want to check.</param>
        /// <returns>Boolean value, true if emailAddress is valid, false otherwise.</returns>
        private static Func<string, bool> IsValid = (emailAddress) => {
            return Regex.IsMatch(emailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        };
        // Converts Email type to string.
        public static implicit operator string(Email email) => email._emailAddress;
        // Takes a string and creates a new Email type with the string as the value of its _emailAddress member.
        public static implicit operator Email(string emailAddress) => new Email(emailAddress);
        /// <summary>
        /// Overrides ToString() method for this class and returns _emailAddress instead of the object type.
        /// </summary>
        /// <returns>String containing the value of _emailAddress.</returns>
        public override string ToString() => _emailAddress;
    }
}
