using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public class CustomerPolicy
    {
        public static int CustomerId { get; set; }
        public static DateTime PolicyEffectiveDate { get; set; }
        public static DateTime PolicyExpiryDate { get; set; }
        public static string PaymentOption { get; set; }
        public static double TotalAmount { get; set; }
        public static string Active { get; set; }
        public static int policyNumber { get; set; }
    }
}

