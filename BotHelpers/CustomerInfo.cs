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

    public class CustomerInfo : CreateCustomer
    {
        //TODO Use one model to hold all properties.
        public static async void AddCustomerDetails(JToken customerDetails)
        {
            try
            {
                foreach (var input in customerDetails)
                {
                    if ((input.ToString()).Contains("FirstName")) CustomerInfo.FirstName = input.First.ToString();
                    if ((input.ToString()).Contains("MiddleName")) CustomerInfo.MiddleName = input.First.ToString();
                    if ((input.ToString()).Contains("LastName")) CustomerInfo.LastName = input.First.ToString();
                    if ((input.ToString()).Contains("ZipCode")) CustomerInfo.ZipCode = input.First.ToString();
                    if ((input.ToString()).Contains("AddressLine1")) CustomerInfo.AddressLine1 = input.First.ToString();
                    if ((input.ToString()).Contains("AddressLine2")) CustomerInfo.AddressLine2 = input.First.ToString();
                    if ((input.ToString()).Contains("State")) CustomerInfo.State = input.First.ToString();
                    if ((input.ToString()).Contains("Email")) CustomerInfo.EmailAddress = input.First.ToString();
                    if ((input.ToString()).Contains("PPC")) CustomerCoverage.PersonalPropertyCoverage = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PLL")) CustomerCoverage.PersonalLiabilityLimit = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PD")) CustomerCoverage.PropertyDeduction = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("DTPOO")) CustomerCoverage.DamageToPropertyOfOthers = Convert.ToDouble(input.First.ToString());
                    if ((input.ToString()).Contains("PolicyEffectiveDate")) CustomerPolicy.PolicyEffectiveDate = Convert.ToDateTime(input.First.ToString());
                    if ((input.ToString()).Contains("PolicyExpiryDate")) CustomerPolicy.PolicyExpiryDate = Convert.ToDateTime(input.First.ToString());
                    if ((input.ToString()).Contains("PO")) CustomerPolicy.PaymentOption = input.First.ToString();
                    if ((input.ToString()).Contains("MobileNumber")) CustomerInfo.MobileNumber = input.First.ToString(); ;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);

            }
        }

    }
}