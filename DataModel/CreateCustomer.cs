using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CreateCustomer
    {
        private CreateCustomer()
        {
        }
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
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string Quote { get; set; }
        public string MobileNumber { get; set; }
        public int PolicyNumber { get; set; }

    }
}
