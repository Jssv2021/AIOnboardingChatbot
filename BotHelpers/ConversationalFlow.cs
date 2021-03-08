using System;
using System.IO;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.DataModel;

namespace ChatbotCustomerOnboarding.BotHelpers
{
    /* Decides the conversation flow based on customer input/
    ///-----------------------------------------------------------------
    ///   Namespace:      <ChatbotCustomerOnboarding>
    ///   Class:          <ConversationalFlow>
    ///   Description:    <  /// *Decides which card to send based on customer input;*/
    /// *Returns Path of the Adaptive Card;*/
    /// *Returns the name of the Thumbnail card*/>
    ///   Author:         <Vignesh Chandran balan>  
    ///-----------------------------------------------------------------

    public class ConversationalFlow
    {
        private const string ExistingCustomer = "ExistingCustomer";
        private const string NewCustomer = "NewCustomer";
        private const string CustomerName = "CustomerNameOK";
        private const string Zip = "ZipCodeOK";
        private const string Dob = "DobOK";
        private const string MailAddress = "MailingAddressOK";
        private const string QuoteCard = "QuoteCardOk";
        private const string CustomerCard = "CustomerCardOk";
        private const string CoverageCard = "CoverageCardOk";
        private const string PolicySummaryCard = "PolicySummaryOk";
        private const string EmailConfirmationOk = "emailConfirmOk";

        public static Func<string> NameCard = () => { return Path.Combine(".", "Resources", "InputCustomerName.json"); };
        public static Func<string> NotSupported = () => { return Path.Combine(".", "Resources", "NotSupported.json"); };
        public static Func<string> ZipCode = () => { return Path.Combine(".", "Resources", "ZipCode.json"); };
        public static Func<string> DateOfBirth = () => { return Path.Combine(".", "Resources", "Dob.json"); };
        public static Func<string> MailingAddress = () => { return Path.Combine(".", "Resources", "MailingAddress.json"); };
        public static Func<string> QuoteRentersInsurance = () => { return Path.Combine(".", "Resources", "QuoteRentersInsurance.json"); };
        public static Func<string> CustomerInformation = () => { return Path.Combine(".", "Resources", "QuoteRentersInsurance.json"); };
        public static Func<string> CoverageOptions = () => { return Path.Combine(".", "Resources", "CoverageOptions.json"); };
        public static Func<string> SendEmailToCustomer = () => { return ("SendEmail"); };
        public static Func<string> PolicySummary = () => { return Path.Combine(".", "Resources", "PolicySummary.json"); };
        public static Func<string> RenterInsurancePolicy = () => { return "RentersInsuranceCard"; };

        public async static Task<string> GetNextCard(dynamic inputMsg)
        {
            var customerType = inputMsg.ToString();
            var NextCardType = customerType switch
            {
                ExistingCustomer => NotSupported(),
                NewCustomer => NameCard(),
                CustomerName => ZipCode(),
                Zip => DateOfBirth(),
                Dob => MailingAddress(),
                MailAddress => Card.QuoteCard,//ThumbNail -QuoteCard
                QuoteCard => Card.EmailConfirmationCard,
                EmailConfirmationOk => CoverageOptions(),
                CoverageCard => PolicySummary(),
                PolicySummaryCard => Card.RentersInsuranceCard,//ThumbNail - RentersInsurance Card
                _ => throw new System.NotImplementedException(),
            };

            return NextCardType;
        }
    }
}
