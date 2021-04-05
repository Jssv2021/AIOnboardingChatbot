using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CustomerPolicy
    {
        private CustomerPolicy()
        {
        }
        private static CustomerPolicy instance = null;
        public static CustomerPolicy Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerPolicy();
                }
                return instance;
            }
        }
        public int CustomerId { get; set; }
        public DateTime PolicyEffectiveDate { get; set; }
        public DateTime PolicyExpiryDate { get; set; }
        public string PaymentOption { get; set; }
        public double TotalAmount { get; set; }
        public string Active { get; set; }
        public int policyNumber { get; set; }
    }
}

