using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;
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

    public class CustomerInfo : GenericHelpers
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

            if (Exists(jResult, "DOB")) { usrInputResult.AppendLine(CreateCustomer.SetDateofBirth(jResult["DOB"].ToString())); }

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