using System;
using static ChatbotCustomerOnboarding.ErrorValidation.Validator;


namespace ChatbotCustomerOnboarding.DataModel
{
    public sealed class CustomerCoverage : GenericHelpers
    {
        private CustomerCoverage()
        {
        }
        private double _personalPropertyCoverage;
        private double _propertyDeduction;
        private double _personalLiabilityLimit;
        private double _damageToPropertyOfOthers;

        public double PersonalPropertyCoverage { get { return _personalPropertyCoverage; } set { _personalPropertyCoverage = value; } }
        public double PropertyDeduction { get { return _propertyDeduction; } set { _propertyDeduction = value; } }
        public double PersonalLiabilityLimit { get { return _personalLiabilityLimit; } set { _personalLiabilityLimit = value; } }
        public double DamageToPropertyOfOthers { get { return _damageToPropertyOfOthers; } set { _damageToPropertyOfOthers = value; } }

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


        public static Func<string, string> SetPersonalPropertyCoverage = (personalPropertyCoverage) =>
        {
            var result = ConvertToDouble(personalPropertyCoverage);
            if (result == defaultDouble) return $"{IsInvalid}:{personalPropertyCoverage} {Errors.Message}: ";
            CustomerCoverage.Instance.PersonalPropertyCoverage = result;
            return Isvalid;
        };


        public static Func<string, string> SetPropertyDeduction = (propertyDeduction) =>
        {
            var result = ConvertToDouble(propertyDeduction);
            if (result == defaultDouble) return $"{IsInvalid}:{propertyDeduction} {Errors.Message}: ";
            CustomerCoverage.Instance.PropertyDeduction = result;
            return Isvalid;
        };


        public static Func<string, string> SetPersonalLiabilityLimit = (personalLiabilityLimit) =>
        {
            var result = ConvertToDouble(personalLiabilityLimit);
            if (result == defaultDouble) return $"{IsInvalid}:{personalLiabilityLimit} {Errors.Message}: ";
            CustomerCoverage.Instance.PersonalLiabilityLimit = result;
            return Isvalid;
        };


        public static Func<string, string> SetDamageToPropertyOfOthers = (damageToPropertyOfOthers) =>
        {
            var result = ConvertToDouble(damageToPropertyOfOthers);
            if (result == defaultDouble) return $"{IsInvalid}:{damageToPropertyOfOthers} {Errors.Message}: ";
            CustomerCoverage.Instance.DamageToPropertyOfOthers = result;
            return Isvalid;
        };

    }
}

