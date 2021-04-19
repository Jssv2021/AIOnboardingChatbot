using System;
using ChatbotCustomerOnboarding.ErrorValidation;
using LaYumba.Functional;
using static ChatbotCustomerOnboarding.ErrorValidation.Validator;

namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CustomerPolicy
    {
        private CustomerPolicy()
        {
        }

        private DateTime _policyEffectiveDate;
        private DateTime _policyExpiryDate;
        private string _paymentOption;


        public DateTime PolicyEffectiveDate { get { return _policyEffectiveDate; } set { _policyEffectiveDate = value; } }
        public DateTime PolicyExpiryDate { get { return _policyExpiryDate; } set { _policyExpiryDate = value; } }
        public string PaymentOption { get { return _paymentOption; } set { _paymentOption = value; } }

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
        public double TotalAmount { get; set; }
        public string Active { get; set; }
        public int policyNumber { get; set; }

        public static Func<string, string> SetPolicyEffectiveDate = (policyEffectiveDate) =>
        {
            var result = Date.Parse(policyEffectiveDate).Match(Some: (e) => e, None: () => defaultTime);

            if (result == defaultTime) return $"{IsInvalid}:policyEffectiveDate {policyEffectiveDate} {Errors.Message}";

            CustomerPolicy.Instance.PolicyEffectiveDate = result;

            return Isvalid;
        };

        public static Func<string, string> SetPolicyExpiryDate = (policyExpiryDate) =>
        {
            var result = Date.Parse(policyExpiryDate).Match(Some: (e) => e, None: () => defaultTime);

            if (result == defaultTime) return $"{IsInvalid}:policyExpiryDate {policyExpiryDate} {Errors.Message}";

            CustomerPolicy.Instance.PolicyExpiryDate = result;

            return Isvalid;
        };

        public static Func<string, string> SetPaymentOption = (paymentOption) =>
        {
            var result = Validator.IsAlpha(paymentOption) ? paymentOption : IsInvalid;

            if (result == IsInvalid) return $"{IsInvalid}: paymentOption {paymentOption} {Errors.Message}";

            CustomerPolicy.Instance.PaymentOption = paymentOption;

            return Isvalid;
        };
    }
}

