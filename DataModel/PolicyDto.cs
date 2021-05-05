using System;
using System.Collections.Generic;
using System.Text;

namespace ChatbotCustomerOnboarding.DataModel
{
    public class PolicyDto
    {
        public string policyNumber { get; set; }
        public string customerId { get; set; }
        public DateTime policyEffectiveDate { get; set; }
        public DateTime policyExpiryDate { get; set; }
        public string paymentOption { get; set; }
        public string totalAmount { get; set; }
        public string active { get; set; }
    }
}
