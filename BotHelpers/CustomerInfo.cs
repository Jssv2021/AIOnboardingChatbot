using System.Text;
using ChatbotCustomerOnboarding.DataModel;
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

    }

}