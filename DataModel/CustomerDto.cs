using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public class CustomerDto
    {
        public string customerId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string zipCode { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string state { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string emailAddress { get; set; }
        public string mobileNumber { get; set; }
        public string button { get; set; }

    }
}
