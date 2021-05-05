using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using ChatbotCustomerOnboarding.DataModel;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LaYumba.Functional;
using System.Net.Http;
using String = System.String;
using System.Collections.Generic;

namespace ChatbotCustomerOnboarding.BotHelpers
{
    using static F;

    /* Add user input to the Customer dto models/
   ///-----------------------------------------------------------------
   ///   Namespace:      <ChatbotCustomerOnboarding>
   ///   Class:          <CustomerInfo>
   ///   Description:    <Read the json response from adaptive cards and set the class property value for customer info & other models>
   ///   Author:         <Vignesh Chandran balan>                    
   /// *Decides which card to send based on customer input;*/
    /// *Returns Path of the Adaptive Card;*/
    /// *Returns the name of the Thumbnail card*/
    ///-----------------------------------------------------------------

    public class CustomerInfo : GenericHelpers
    {
        public static Func<string, Task<Option<CustomerDto>>> GetCustomerAsync = async (emailAddress) =>
        {
            Option<Email> email = Email.Validate(emailAddress); //Run Validate function to ensure email address is correct
            Option<string> customerJson =  
                await email.Match(
                    None: async () => await ReportEmailError(),
                    Some: async (e) => await GetCustomerJsonByEmailAsync(e.ToString())
                );
            Option<CustomerDto> customer = customerJson.Bind(e => ConvertToObject<CustomerDto>(e));
            return customer;
        };

        private static Func<Task<string>> ReportEmailError = async () => "There was an error with the email address submitted.";

        private static Func<string, Task<string>> GetCustomerJsonByEmailAsync = async (e) =>
        {
            IAPIHelper Invoke = new APIHelper();
            Option<HttpResponseMessage> customerInfoResponse = await Invoke.GetAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer/GetByEmail/", e, HttpStatusCode.OK);
            return
                await customerInfoResponse.Match(
                    None: () => ReportNotFound(),
                    Some: async (r) => await r.Content.ReadAsStringAsync()
                );
        };

        private static Func<Task<string>> ReportNotFound = async () => "That email address does not belong to a current customer.";

        //TODO Use one model to hold all properties.
        public static string AddCustomerDetails(JToken customerDetails)
        {
            var jResult = JObjectParse(customerDetails.ToString());
            var usrInputResult = new StringBuilder();
            if (Exists(jResult, "FirstName")) { usrInputResult.AppendLine(CreateCustomer.SetFirstName(jResult["FirstName"].ToString())); }
            if (Exists(jResult, "MiddleName")) { usrInputResult.AppendLine(CreateCustomer.SetMiddleName(jResult["MiddleName"].ToString())); }
            if (Exists(jResult, "LastName")) { usrInputResult.AppendLine(CreateCustomer.SetLastName(jResult["LastName"].ToString())); }
            if (Exists(jResult, "ZipCode")) { usrInputResult.AppendLine(CreateCustomer.SetZipCode(jResult["ZipCode"].ToString())); }
            if (Exists(jResult, "AddressLine1")) { usrInputResult.AppendLine(CreateCustomer.SetAddressLine1(jResult["AddressLine1"].ToString())); }
            if (Exists(jResult, "AddressLine2")) { usrInputResult.AppendLine(CreateCustomer.SetAddressLine2(jResult["AddressLine2"].ToString())); }
            if (Exists(jResult, "State")) { usrInputResult.AppendLine(CreateCustomer.SetState(jResult["State"].ToString())); }
            if (Exists(jResult, "EmailAddress")) { usrInputResult.AppendLine(CreateCustomer.SetEmail(jResult["EmailAddress"].ToString())); }
            if (Exists(jResult, "MobileNumber")) { usrInputResult.AppendLine(CreateCustomer.SetMobileNumber(jResult["MobileNumber"].ToString())); }
            if (Exists(jResult, "DateOfBirth")) { usrInputResult.AppendLine(CreateCustomer.SetDateofBirth(jResult["DateOfBirth"].ToString())); }
            if (Exists(jResult, "PPC")) { usrInputResult.AppendLine(CustomerCoverage.SetPersonalPropertyCoverage(jResult["PPC"].ToString())); }
            if (Exists(jResult, "PLL")) { usrInputResult.AppendLine(CustomerCoverage.SetPersonalLiabilityLimit(jResult["PLL"].ToString())); }
            if (Exists(jResult, "PD")) { usrInputResult.AppendLine(CustomerCoverage.SetPropertyDeduction(jResult["PD"].ToString())); }
            if (Exists(jResult, "DTPOO")) { usrInputResult.AppendLine(CustomerCoverage.SetDamageToPropertyOfOthers(jResult["DTPOO"].ToString())); }
            if (Exists(jResult, "PolicyEffectiveDate")) { usrInputResult.AppendLine(CustomerPolicy.SetPolicyEffectiveDate(jResult["PolicyEffectiveDate"].ToString())); }
            if (Exists(jResult, "PolicyExpiryDate")) { usrInputResult.AppendLine(CustomerPolicy.SetPolicyExpiryDate(jResult["PolicyExpiryDate"].ToString())); }
            if (Exists(jResult, "PO")) { usrInputResult.AppendLine(CustomerPolicy.SetPaymentOption(jResult["PO"].ToString())); }
            return usrInputResult.ToString();
        }

        public static async void UpdateCustomerDetails(CustomerDto customerDetails)
        {
            CreateCustomer.Instance.CustomerId = Convert.ToInt32(ThisOrThat(CreateCustomer.Instance.CustomerId.ToString(), customerDetails.customerId));
            CreateCustomer.Instance.FirstName = ThisOrThat(CreateCustomer.Instance.FirstName, customerDetails.firstName);
            CreateCustomer.Instance.MiddleName = ThisOrThat(CreateCustomer.Instance.MiddleName, customerDetails.middleName);
            CreateCustomer.Instance.LastName = ThisOrThat(CreateCustomer.Instance.LastName, customerDetails.lastName);
            CreateCustomer.Instance.ZipCode = ThisOrThat(CreateCustomer.Instance.ZipCode, customerDetails.zipCode);
            CreateCustomer.Instance.DateOfBirth = ThisOrThat(CreateCustomer.Instance.DateOfBirth, customerDetails.dateOfBirth);
            CreateCustomer.Instance.AddressLine1 = ThisOrThat(CreateCustomer.Instance.AddressLine1, customerDetails.addressLine1);
            CreateCustomer.Instance.AddressLine2 = ThisOrThat(CreateCustomer.Instance.AddressLine2, customerDetails.addressLine2);
            CreateCustomer.Instance.State = ThisOrThat(CreateCustomer.Instance.State, customerDetails.state);
            CreateCustomer.Instance.MobileNumber = ThisOrThat(CreateCustomer.Instance.MobileNumber, customerDetails.mobileNumber);
            CreateCustomer.Instance.EmailAddress = ThisOrThat(CreateCustomer.Instance.EmailAddress, customerDetails.emailAddress);
        }

        public static async void UpdateCoverageDetails(CoverageDto coverageDetails)
        {
            CustomerCoverage.Instance.PersonalPropertyCoverage = ThisOrThat(CustomerCoverage.Instance.PersonalPropertyCoverage, coverageDetails.personalPropertyCoverage);
            CustomerCoverage.Instance.PersonalLiabilityLimit = ThisOrThat(CustomerCoverage.Instance.PersonalLiabilityLimit, coverageDetails.personalLiabilityLimit);
            CustomerCoverage.Instance.PropertyDeduction = ThisOrThat(CustomerCoverage.Instance.PropertyDeduction, coverageDetails.propertyDeduction);
            CustomerCoverage.Instance.DamageToPropertyOfOthers = ThisOrThat(CustomerCoverage.Instance.DamageToPropertyOfOthers, coverageDetails.damageToPropertyOfOthers);
        }

        public static async void UpdatePolicyDetails(PolicyDto policyDetails)
        {
            CustomerPolicy.Instance.CustomerId = Convert.ToInt32(ThisOrThat(CreateCustomer.Instance.CustomerId.ToString(), policyDetails.customerId));
            //CustomerPolicy.Instance.PolicyNumber = Convert.ToInt32(ThisOrThat(CreateCustomer.Instance.PolicyNumber.ToString(), policyDetails.policyNumber));
            CustomerPolicy.Instance.PolicyEffectiveDate = ThisOrThat(CustomerPolicy.Instance.PolicyEffectiveDate, policyDetails.policyEffectiveDate);
            CustomerPolicy.Instance.PolicyExpiryDate = ThisOrThat(CustomerPolicy.Instance.PolicyExpiryDate, policyDetails.policyExpiryDate);
            CustomerPolicy.Instance.PaymentOption = ThisOrThat(CustomerPolicy.Instance.PaymentOption, policyDetails.paymentOption);
            CustomerPolicy.Instance.TotalAmount = Convert.ToDouble(ThisOrThat(CustomerPolicy.Instance.TotalAmount.ToString(), policyDetails.totalAmount));
            CustomerPolicy.Instance.Active = ThisOrThat(CustomerPolicy.Instance.Active, policyDetails.active);
        }

        //private static string ThisOrThat(string @this, string@that)
        //{
        //    Option<string> val = String.IsNullOrEmpty(that) ? None : Some(that);
        //    return val.Match(
        //        None: () => @this,
        //        Some: (t) => t
        //    );
        //}

        private static string ThisOrThat(string @this, DateTime @that)
        {
            Option<string> val = that.ToString("yyyy-MM-ddTHH:mm:sszz z") == "0001-01-01T00:00:00-06:00" ? None : Some(that.ToString("yyyy-MM-ddTHH:mm:sszzz"));
            return val.Match(
                None: () => @this,
                Some: (t) => t
            );
        }

        private static DateTime ThisOrThat(DateTime @this, DateTime @that)
        {
            Option<DateTime> val = that.ToString("yyyy-MM-ddTHH:mm:sszz z") == "0001-01-01T00:00:00-06:00" ? None : Some(that);
            return val.Match(
                None: () => @this,
                Some: (t) => t
            );
        }

        private static T ThisOrThat<T>(T @this, T @that)
        {
            Option<T> val = ((that == null) || that.Equals(default)) ? None : Some(that);
            return val.Match(
                None: () => @this,
                Some: (t) => t
            );
        }

        public static async Task<string> UpdateAndSave(string customerId)
        {
            IAPIHelper Invoke = new APIHelper();
            JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "CreateCustomer.json")));
            customerRecord["zipCode"] = ThisOrThat(CreateCustomer.Instance.ZipCode, String.Empty);
            customerRecord["firstName"] = ThisOrThat(CreateCustomer.Instance.FirstName, String.Empty);
            customerRecord["middleName"] = ThisOrThat(CreateCustomer.Instance.MiddleName, String.Empty);
            customerRecord["lastName"] = ThisOrThat(CreateCustomer.Instance.LastName, String.Empty);
            customerRecord["dateOfBirth"] = ThisOrThat(CreateCustomer.Instance.DateOfBirth, String.Empty);
            customerRecord["addressLine1"] = ThisOrThat(CreateCustomer.Instance.AddressLine1, String.Empty);
            customerRecord["addressLine2"] = ThisOrThat(CreateCustomer.Instance.AddressLine2, String.Empty);
            customerRecord["state"] = ThisOrThat(CreateCustomer.Instance.State, String.Empty);
            customerRecord["mobileNumber"] = ThisOrThat(CreateCustomer.Instance.MobileNumber, String.Empty);
            customerRecord["emailAddress"] = ThisOrThat(CreateCustomer.Instance.EmailAddress, String.Empty);
            Option<HttpResponseMessage> response = await Invoke.PutAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer/", customerId.ToString(), HttpStatusCode.OK, customerRecord.ToString());
            string customerJson = 
                await response.Match(
                    None: () => ReportNotFound(),
                    Some: async (r) => await r.Content.ReadAsStringAsync()
                );
            Option<GetCustomer> customer = ConvertToObject<GetCustomer>(customerJson);
            CreateCustomer.Instance.CustomerId = customer.Match(
                None: () => 0,
                Some: (c) => Convert.ToInt32(c.CustomerId)
            );
            return customer.Match(
                None: () => "An error occurred. Your information has not been updated.",
                Some: (c) => c.CustomerId.ToString()
            );
        }
    }
}