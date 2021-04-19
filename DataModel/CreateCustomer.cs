using System;
using ChatbotCustomerOnboarding.ErrorValidation;
using LaYumba.Functional;
using static ChatbotCustomerOnboarding.ErrorValidation.Validator;

namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CreateCustomer
    {
        private CreateCustomer()
        {
        }

        /*fields*/
        private string _firstName;
        private string _lastName;
        private string _middleName;
        private string _zipCode;
        private string _addressLine1;
        private string _addressLine2;
        private string _state;
        private string _dateofbirth;
        private string _mobileNmber;
        private string _emailAddress;

        /*Class instance*/
        private static CreateCustomer instance = null;
        public static CreateCustomer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CreateCustomer();
                }
                return instance;
            }
        }

        /*Properties*/
        public int CustomerId { get; set; }
        public string FirstName { get { return _firstName; } set { _firstName = value; } }
        public string MiddleName { get { return _middleName; } set { _middleName = value; } }
        public string LastName { get { return _lastName; } set { _lastName = value; } }
        public string ZipCode { get { return _zipCode; } set { _zipCode = value; } }
        public string AddressLine1 { get { return _addressLine1; } set { _addressLine1 = value; } }
        public string AddressLine2 { get { return _addressLine2; } set { _addressLine2 = value; } }
        public string State { get { return _state; } set { _state = value; } }
        public string DateOfBirth { get { return _dateofbirth; } set { _dateofbirth = value; } }
        public string EmailAddress { get { return _emailAddress; } set { _emailAddress = value; } }
        public string MobileNumber { get { return _mobileNmber; } set { _mobileNmber = value; } }

        public string Quote { get; set; }
        public int PolicyNumber { get; set; }

        public static Func<string, string> SetEmail = (emailAddress) =>
        {
            var result = Email.Create(emailAddress).Match(Some: (e) => e, None: () => IsInvalid);

            if (result == IsInvalid) return $"{IsInvalid}:Email ID entered {emailAddress} {Errors.Message}: ";

            CreateCustomer.Instance.EmailAddress = emailAddress;

            return Isvalid;
        };

        public static Func<string, string> SetFirstName = (name) =>
        {
            var result = Validator.IsAlpha(name) ? name : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: First Name: {name} {Errors.Message}";

            CreateCustomer.Instance.FirstName = name;

            return Isvalid;
        };

        public static Func<string, string> SetMiddleName = (name) =>
        {
            string result = "";

            if (name.Length > 0) { result = Validator.IsAlpha(name) ? name : IsInvalid; }

            if (result == IsInvalid) return $"{IsInvalid}: MiddleName {name} {Errors.Message}";

            CreateCustomer.Instance.MiddleName = name;

            return Isvalid;
        };

        public static Func<string, string> SetLastName = (name) =>
        {
            var result = Validator.IsAlpha(name) ? name : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: LastName {name} {Errors.Message}: ";

            CreateCustomer.Instance.LastName = name;

            return Isvalid;
        };

        public static Func<string, string> SetState = (state) =>
        {
            var result = Validator.IsAlpha(state) ? state : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: state {state} {Errors.Message}";

            CreateCustomer.Instance.State = state;

            return Isvalid;
        };


        public static Func<string, string> SetZipCode = (zipCode) =>
        {
            var result = Validator.IsZipCode(zipCode) ? zipCode : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: zipCode {zipCode} {Errors.Message}";

            CreateCustomer.Instance.ZipCode = zipCode;

            return Isvalid;
        };

        public static Func<string, string> SetDateofBirth = (dob) =>
        {
            var result = Date.Parse(dob).Match(Some: (e) => e, None: () => defaultTime);

            if (result == defaultTime) return $"{IsInvalid}:dob {dob} {Errors.Message}";

            CreateCustomer.Instance.DateOfBirth = dob;

            return Isvalid;
        };


        public static Func<string, string> SetMobileNumber = (mobileNumber) =>
        {
            var result = Validator.IsMobile(mobileNumber) ? mobileNumber : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: mobileNumber {mobileNumber} {Errors.Message}";

            CreateCustomer.Instance.MobileNumber = mobileNumber;

            return Isvalid;
        };

        public static Func<string, string> SetAddressLine1 = (line1) =>
        {
            var result = Validator.IsAddress(line1) ? line1 : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}:line1 {line1} {Errors.Message}";

            CreateCustomer.Instance.AddressLine1 = line1;

            return Isvalid;
        };

        public static Func<string, string> SetAddressLine2 = (line2) =>
        {
            var result = Validator.IsAphaNumeric(line2) ? line2 : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}:line2 {line2} {Errors.Message}";

            CreateCustomer.Instance.AddressLine2 = line2;

            return Isvalid;
        };


    }

}
