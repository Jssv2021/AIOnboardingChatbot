using System;
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

    public class CustomerInfo
    {
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

    }
}