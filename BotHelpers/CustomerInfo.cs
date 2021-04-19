using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.DataModel;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatbotCustomerOnboarding.BotHelpers
{
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

    public class CustomerInfo
    {
        public async static Task<CustomerDto> GetCustomer(string emailAddress)
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                var customerInfoResponse = await Invoke.GetAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer/GetByEmail/", emailAddress, HttpStatusCode.OK);
                string customerInfoJson = await customerInfoResponse.Content.ReadAsStringAsync();
                if (customerInfoJson != "")
                {
                    CustomerDto getCustomer = JsonConvert.DeserializeObject<CustomerDto>(customerInfoJson);
                    return getCustomer;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); //Replace with Logger TODO
                return null;
            }
        }

        //TODO Use one model to hold all properties.
        public static async void AddCustomerDetails(JToken customerDetails)
        {
            try
            {
                foreach (var input in customerDetails)
                {
                    if ((input.ToString()).Contains("FirstName")) CreateCustomer.Instance.FirstName = input.First.ToString();
                    if ((input.ToString()).Contains("MiddleName")) CreateCustomer.Instance.MiddleName = input.First.ToString();
                    if ((input.ToString()).Contains("LastName")) CreateCustomer.Instance.LastName = input.First.ToString();
                    if ((input.ToString()).Contains("ZipCode")) CreateCustomer.Instance.ZipCode = input.First.ToString();
                    if ((input.ToString()).Contains("AddressLine1")) CreateCustomer.Instance.AddressLine1 = input.First.ToString();
                    if ((input.ToString()).Contains("AddressLine2")) CreateCustomer.Instance.AddressLine2 = input.First.ToString();
                    if ((input.ToString()).Contains("State")) CreateCustomer.Instance.State = input.First.ToString();
                    if ((input.ToString()).Contains("Email")) CreateCustomer.Instance.EmailAddress = input.First.ToString();
                    if ((input.ToString()).Contains("PPC")) CustomerCoverage.Instance.PersonalPropertyCoverage = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PLL")) CustomerCoverage.Instance.PersonalLiabilityLimit = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PD")) CustomerCoverage.Instance.PropertyDeduction = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("DTPOO")) CustomerCoverage.Instance.DamageToPropertyOfOthers = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PolicyEffectiveDate")) CustomerPolicy.Instance.PolicyEffectiveDate = Convert.ToDateTime(input.First.ToString());
                    if ((input.ToString()).Contains("PolicyExpiryDate")) CustomerPolicy.Instance.PolicyExpiryDate = Convert.ToDateTime(input.First.ToString());
                    if ((input.ToString()).Contains("PO")) CustomerPolicy.Instance.PaymentOption = input.First.ToString();
                    if ((input.ToString()).Contains("MobileNumber")) CreateCustomer.Instance.MobileNumber = input.First.ToString(); ;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

        //TODO Use one model to hold all properties.
        public static async void UpdateCustomerDetails(CustomerDto customerDetails)
        {
            try
            {
                CreateCustomer.Instance.FirstName = !String.IsNullOrEmpty(customerDetails.firstName) ? customerDetails.firstName : CreateCustomer.Instance.FirstName;
                CreateCustomer.Instance.MiddleName = !String.IsNullOrEmpty(customerDetails.middleName) ? customerDetails.middleName : CreateCustomer.Instance.MiddleName;
                CreateCustomer.Instance.LastName = !String.IsNullOrEmpty(customerDetails.lastName) ? customerDetails.lastName : CreateCustomer.Instance.LastName;
                CreateCustomer.Instance.ZipCode = !String.IsNullOrEmpty(customerDetails.zipCode) ? customerDetails.zipCode : CreateCustomer.Instance.ZipCode;
                CreateCustomer.Instance.AddressLine1 = !String.IsNullOrEmpty(customerDetails.addressLine1) ? customerDetails.addressLine1 : CreateCustomer.Instance.AddressLine1;
                CreateCustomer.Instance.AddressLine2 = !String.IsNullOrEmpty(customerDetails.addressLine2) ? customerDetails.addressLine2 : CreateCustomer.Instance.AddressLine2;
                CreateCustomer.Instance.State = !String.IsNullOrEmpty(customerDetails.state) ? customerDetails.state : CreateCustomer.Instance.State;
                CreateCustomer.Instance.MobileNumber = !String.IsNullOrEmpty(customerDetails.mobileNumber) ? customerDetails.mobileNumber : CreateCustomer.Instance.MobileNumber;
                CreateCustomer.Instance.EmailAddress = !String.IsNullOrEmpty(customerDetails.emailAddress) ? customerDetails.emailAddress : CreateCustomer.Instance.EmailAddress;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task<string> UpdateAndSave(string customerId)
        {
            try
            {
                IAPIHelper Invoke = new APIHelper();
                JObject customerRecord = JObject.Parse(File.ReadAllText(Path.Combine(".", "JsonTemplate", "CreateCustomer.json")));
                //TODO 
                customerRecord["zipCode"] = String.IsNullOrEmpty(CreateCustomer.Instance.ZipCode) ? String.Empty : CreateCustomer.Instance.ZipCode;
                customerRecord["firstName"] = String.IsNullOrEmpty(CreateCustomer.Instance.FirstName) ? String.Empty : CreateCustomer.Instance.FirstName;
                customerRecord["middleName"] = String.IsNullOrEmpty(CreateCustomer.Instance.MiddleName) ? String.Empty : CreateCustomer.Instance.MiddleName;
                customerRecord["lastName"] = String.IsNullOrEmpty(CreateCustomer.Instance.LastName) ? String.Empty : CreateCustomer.Instance.LastName;
                customerRecord["addressLine1"] = String.IsNullOrEmpty(CreateCustomer.Instance.AddressLine1) ? String.Empty : CreateCustomer.Instance.AddressLine1;
                customerRecord["addressLine2"] = String.IsNullOrEmpty(CreateCustomer.Instance.AddressLine2) ? String.Empty : CreateCustomer.Instance.AddressLine2;
                customerRecord["state"] = String.IsNullOrEmpty(CreateCustomer.Instance.State) ? String.Empty : CreateCustomer.Instance.State;
                customerRecord["mobileNumber"] = String.IsNullOrEmpty(CreateCustomer.Instance.MobileNumber) ? String.Empty : CreateCustomer.Instance.MobileNumber;
                customerRecord["emailAddress"] = String.IsNullOrEmpty(CreateCustomer.Instance.EmailAddress) ? String.Empty : CreateCustomer.Instance.EmailAddress;
                
                var getCustomer = await Invoke.PutAPI("https://ai-customer-onboarding-dev.azurewebsites.net/api/Customer/", customerId.ToString(), HttpStatusCode.OK, customerRecord.ToString());
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
    }
}