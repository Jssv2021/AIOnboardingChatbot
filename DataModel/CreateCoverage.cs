using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CustomerCoverage
    {
        private CustomerCoverage()
        {
        }
        private static CustomerCoverage instance = null;
        public static CustomerCoverage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomerCoverage();
                }
                return instance;
            }
        }
        public double PersonalPropertyCoverage { get; set; }
        public double PropertyDeduction { get; set; }
        public double PersonalLiabilityLimit { get; set; }
        public double DamageToPropertyOfOthers { get; set; }
    }
}

