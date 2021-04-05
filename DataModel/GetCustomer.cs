using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class GetCustomer
    {
        private GetCustomer()
        {
        }
        private static GetCustomer instance = null;
        public static GetCustomer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GetCustomer();
                }
                return instance;
            }
        }
        public string Quote { get; set; }
        public string CustomerId { get; set; }
        public int PolicyNumber { get; set; }
    }
}
