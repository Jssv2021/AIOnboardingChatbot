using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.DataModel;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/* Class to Interact with Rest API services/
   ///-----------------------------------------------------------------
   ///   Namespace:      <ChatbotCustomerOnboarding>
   ///   Class:          <QuoteCard>
   ///   Description:    <Card which displays customer Quote and Invoke rest api to create customers>
   ///   Author:         <Vignesh Chandran balan>                    
 ///-----------------------------------------------------------------*/

namespace ChatbotCustomerOnboarding.BotHelpers
{
    public class QuoteCard
    {
        public async static Task<dynamic> GetQuote()
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                var getQuoteRate = await Invoke.GetAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/InsuranceQuote/", $"{CustomerInfo.ZipCode}", HttpStatusCode.OK);
                string getQuoteJson = await getQuoteRate.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getQuote = JsonConvert.DeserializeObject<GetQuote>(getQuoteJson.ToString());
                    CustomerPolicy.TotalAmount = (Convert.ToDouble(getQuote.Quote) * 12);
                    return getQuote.Quote;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Replace with Logger TODO
                return null;
            }
        }

        public async static Task<string> CreateCustomer()
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "CreateCustomer.json")));
                //TODO 
                customerRecord["zipCode"] = CustomerInfo.ZipCode;
                customerRecord["firstName"] = CustomerInfo.FirstName;
                customerRecord["lastName"] = CustomerInfo.LastName;
                customerRecord["streetAddress"] = $"{CustomerInfo.AddressLine1}{CustomerInfo.AddressLine2}";
                customerRecord["mobileNumber"] = CustomerInfo.MobileNumber;
                customerRecord["emailAddress"] = CustomerInfo.EmailAddress;
                var getCustomer = await Invoke.PostAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer", "", HttpStatusCode.Created, customerRecord.ToString());
                string getQuoteJson = await getCustomer.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getCustomerInfo = JsonConvert.DeserializeObject<GetCustomer>(getQuoteJson.ToString());
                    CustomerInfo.CustomerId = Convert.ToInt32(getCustomerInfo.CustomerId);
                    return getCustomerInfo.CustomerId.ToString();

                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Replace with Logger TODO
                return null;
            }
        }

        public async static Task<string> AddCoverage()
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "AddCoverage.json")));
                //TODO
                customerRecord["customerId"] = CustomerInfo.CustomerId;
                customerRecord["personalPropertyCoverage"] = CustomerCoverage.PersonalLiabilityLimit;
                customerRecord["propertyDeduction"] = CustomerCoverage.PropertyDeduction;
                customerRecord["personalLiabilityLimit"] = CustomerCoverage.PersonalLiabilityLimit;
                customerRecord["damageToPropertyOfOthers"] = CustomerCoverage.DamageToPropertyOfOthers;

                var getCustomer = await Invoke.PostAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Coverage", "", HttpStatusCode.Created, customerRecord.ToString());
                string getQuoteJson = await getCustomer.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getCustomerInfo = JsonConvert.DeserializeObject<CustomerCoverage>(getQuoteJson.ToString());
                    return getCustomerInfo.ToString();
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Replace with Logger TODO
                return null;
            }
        }

        public async static Task<string> CreatePolicy()
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "CreatePolicy.json")));
                //TODO
                customerRecord["customerId"] = CustomerInfo.CustomerId;
                customerRecord["policyEffectiveDate"] = CustomerPolicy.PolicyEffectiveDate;
                customerRecord["policyExpiryDate"] = CustomerPolicy.PolicyExpiryDate;
                customerRecord["paymentOption"] = CustomerPolicy.PaymentOption;
                customerRecord["totalAmount"] = CustomerPolicy.TotalAmount;

                var getCustomer = await Invoke.PostAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Policy", "", HttpStatusCode.Created, customerRecord.ToString());
                string getQuoteJson = await getCustomer.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getCustomerInfo = JsonConvert.DeserializeObject<GetCustomer>(getQuoteJson.ToString());
                    CustomerInfo.PolicyNumber = getCustomerInfo.PolicyNumber;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Replace with Logger TODO
                return null;
            }
        }

        public static Attachment QuoteSummary()
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = $"Renters Insurance Quote - EstimatedTotal: {GetCustomer.Quote} per month;",
                Subtitle = "Personal Details",
                Text = $"Name:{CustomerInfo.FirstName} {CustomerInfo.MiddleName} {CustomerInfo.LastName}; Address: {CustomerInfo.AddressLine1}, {CustomerInfo.AddressLine2},State: {CustomerInfo.State}, ZipCode: {CustomerInfo.ZipCode}",
                Buttons = new[] {new CardAction(
                        ActionTypes.MessageBack, $"Ok", value: "QuoteCardOk"
                        ) }

            };

            return thumbnailCard.ToAttachment();
        }

        public static Attachment RentersInsuranceSummary()
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = $"Renters insurance has been successfully Purchased:",
                Subtitle = $"Customer ID: {CustomerInfo.CustomerId},PolicyNo: {CustomerInfo.PolicyNumber}",
                Text = $"Name:{CustomerInfo.FirstName} {CustomerInfo.MiddleName} {CustomerInfo.LastName}; " +
                $"Address: {CustomerInfo.AddressLine1}, " +
                $"{CustomerInfo.AddressLine2}," +
                $"State: {CustomerInfo.State}, " +
                $"ZipCode: {CustomerInfo.ZipCode}," +
                $"Total Amount:{CustomerPolicy.TotalAmount}",
                Buttons = new[] {new CardAction(
                        ActionTypes.MessageBack, $"Ok", value: "CustomerOk"
                        ) }
            };

            return thumbnailCard.ToAttachment();
        }

        public static Attachment EmailConfirmation()
        {
            var thumbnailCard = new ThumbnailCard
            {
                Title = $"An email has been sent with quote information:",
                Subtitle = $"Customer ID:{CustomerInfo.CustomerId}, {CustomerInfo.EmailAddress}",
                Text = $"Name:{CustomerInfo.FirstName} {CustomerInfo.MiddleName} {CustomerInfo.LastName}; Address: {CustomerInfo.AddressLine1}, {CustomerInfo.AddressLine2},State: {CustomerInfo.State}, ZipCode: {CustomerInfo.ZipCode}",
                Buttons = new[] {new CardAction(
                        ActionTypes.MessageBack, $"Ok", value: "emailConfirmOk"
                        ) }
            };

            return thumbnailCard.ToAttachment();
        }
    }
}




