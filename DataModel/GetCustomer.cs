using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace ChatbotCustomerOnboarding.DataModel
{
    public class GetCustomer
    {
        public static string Quote { get; set; }
        public string CustomerId { get; set; }
        public int PolicyNumber { get; set; }
    }
}
