using System;
using System.Collections.Generic;
using System.Text;

namespace ChatbotCustomerOnboarding.DataModel
{
    public class CoverageDto
    {
        public string customerId { get; set; }
        public double personalPropertyCoverage { get; set; }
        public double propertyDeduction { get; set; }
        public double personalLiabilityLimit { get; set; }
        public double damageToPropertyOfOthers { get; set; }
    }
}
