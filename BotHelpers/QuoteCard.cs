using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.DataModel;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ChatbotCustomerOnboarding.ErrorValidation.Validator;

/* Class to Interact with Rest API services/
   ///-----------------------------------------------------------------
   ///   Namespace:      <ChatbotCustomerOnboarding>
   ///   Class:          <QuoteCard>
   ///   Description:    <Card which displays customer Quote and Invoke rest api to create customers>
   ///   Author:         <Vignesh Chandran balan>                    
 ///-----------------------------------------------------------------*/

namespace ChatbotCustomerOnboarding.BotHelpers
{
    public class QuoteCard : GenericHelpers
    {
        public async static Task<dynamic> GetQuote()
        {

            IAPIHelper Invoke = new APIHelper();
            var getQuoteRate = await Invoke.GetAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/InsuranceQuote/", $"{CreateCustomer.Instance.ZipCode}", HttpStatusCode.OK);

            if (getQuoteRate != null && getQuoteRate.StatusCode == HttpStatusCode.OK)
            {
                string getQuoteJson = await getQuoteRate.Content.ReadAsStringAsync();
                var getQuote = Deserialize<GetQuote>(getQuoteJson.ToString());
                CustomerPolicy.Instance.TotalAmount = (Convert.ToDouble(getQuote.Quote) * 12);
                return getQuote.Quote;
            }
            return $"{IsInvalid} Quote Not Found for the provided ZipCode";
        }

        public async static Task<string> CreateCustomerRentersInsurance()
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "CreateCustomer.json")));
                //TODO 
                customerRecord["zipCode"] = CreateCustomer.Instance.ZipCode;
                customerRecord["firstName"] = CreateCustomer.Instance.FirstName;
                customerRecord["middleName"] = CreateCustomer.Instance.MiddleName;
                customerRecord["lastName"] = CreateCustomer.Instance.LastName;
                customerRecord["addressLine1"] = $"{CreateCustomer.Instance.AddressLine1}";
                customerRecord["addressLine2"] = $"{CreateCustomer.Instance.AddressLine2}";
                customerRecord["state"] = $"{CreateCustomer.Instance.State}";
                customerRecord["mobileNumber"] = CreateCustomer.Instance.MobileNumber;
                customerRecord["emailAddress"] = CreateCustomer.Instance.EmailAddress;
                var getCustomer = await Invoke.PostAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer", "", HttpStatusCode.Created, customerRecord.ToString());
                string getQuoteJson = await getCustomer.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getCustomerInfo = JsonConvert.DeserializeObject<GetCustomer>(getQuoteJson.ToString());
                    CreateCustomer.Instance.CustomerId = Convert.ToInt32(getCustomerInfo.CustomerId);
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
                customerRecord["customerId"] = CreateCustomer.Instance.CustomerId;
                customerRecord["personalPropertyCoverage"] = CustomerCoverage.Instance.PersonalLiabilityLimit;
                customerRecord["propertyDeduction"] = CustomerCoverage.Instance.PropertyDeduction;
                customerRecord["personalLiabilityLimit"] = CustomerCoverage.Instance.PersonalLiabilityLimit;
                customerRecord["damageToPropertyOfOthers"] = CustomerCoverage.Instance.DamageToPropertyOfOthers;

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
                customerRecord["customerId"] = CreateCustomer.Instance.CustomerId;
                customerRecord["policyEffectiveDate"] = CustomerPolicy.Instance.PolicyEffectiveDate;
                customerRecord["policyExpiryDate"] = CustomerPolicy.Instance.PolicyExpiryDate;
                customerRecord["paymentOption"] = CustomerPolicy.Instance.PaymentOption;
                customerRecord["totalAmount"] = CustomerPolicy.Instance.TotalAmount;

                var getCustomer = await Invoke.PostAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Policy", "", HttpStatusCode.Created, customerRecord.ToString());
                string getQuoteJson = await getCustomer.Content.ReadAsStringAsync();
                if (getQuoteJson != "")
                {
                    var getCustomerInfo = JsonConvert.DeserializeObject<GetCustomer>(getQuoteJson.ToString());
                    CreateCustomer.Instance.PolicyNumber = getCustomerInfo.PolicyNumber;
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
                Title = $"Renters Insurance Quote - EstimatedTotal: {GetCustomer.Instance.Quote} per month;",
                Subtitle = "Personal Details",
                Text = $"Name:{CreateCustomer.Instance.FirstName} {CreateCustomer.Instance.MiddleName} {CreateCustomer.Instance.LastName}; Address: {CreateCustomer.Instance.AddressLine1}, {CreateCustomer.Instance.AddressLine2},State: {CreateCustomer.Instance.State}, ZipCode: {CreateCustomer.Instance.ZipCode}",
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
                Subtitle = $"Customer ID: {CreateCustomer.Instance.CustomerId},PolicyNo: {CreateCustomer.Instance.PolicyNumber}",
                Text = $"Name:{CreateCustomer.Instance.FirstName} {CreateCustomer.Instance.MiddleName} {CreateCustomer.Instance.LastName}; " +
                $"Address: {CreateCustomer.Instance.AddressLine1}, " +
                $"{CreateCustomer.Instance.AddressLine2}," +
                $"State: {CreateCustomer.Instance.State}, " +
                $"ZipCode: {CreateCustomer.Instance.ZipCode}," +
                $"Total Amount:{CustomerPolicy.Instance.TotalAmount}",
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
                Subtitle = $"Customer ID:{CreateCustomer.Instance.CustomerId}, {CreateCustomer.Instance.EmailAddress}",
                Text = $"Name:{CreateCustomer.Instance.FirstName} {CreateCustomer.Instance.MiddleName} {CreateCustomer.Instance.LastName}; Address: {CreateCustomer.Instance.AddressLine1}, {CreateCustomer.Instance.AddressLine2},State: {CreateCustomer.Instance.State}, ZipCode: {CreateCustomer.Instance.ZipCode}",
                Buttons = new[] {new CardAction(
                        ActionTypes.MessageBack, $"Ok", value: "emailConfirmOk"
                        ) }
            };

            return thumbnailCard.ToAttachment();
        }
    }
}




