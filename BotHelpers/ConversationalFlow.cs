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
        private const string UpdateCustomerNameCard = "UpdateCustomerNameOK";
        private const string UpdateZipCodeCard = "UpdateZipCodeOK";
        private const string UpdateDobCard = "UpdateDobOK";
        private const string UpdateMailingAddressCard = "UpdateMailingAddressOK";
        private const string UpdateFinalCard = "UpdateFinalOK";
        private const string UpdateQuoteCard = "UpdateQuoteCardOk";
        private const string UpdateCustomerCard = "UpdateCustomerCardOk";
        private const string UpdateCoverageCard = "UpdateCoverageCardOk";
        private const string UpdatePolicySummaryCard = "UpdatePolicySummaryOk";
        private const string UpdateEmailConfirmationOk = "UpdateEmailConfirmOk";
        private const string NewCustomer = "NewCustomer";
        private const string LoginWithEmailCard = "LoginWithEmailNotFoundOK";
        private const string LoginWithEmailNotFoundCard = "LoginWithEmailOK";
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
        public static Func<string> LoginWithEmail = () => { return Path.Combine(".", "Resources", "LoginWithEmail.json"); };
        public static Func<string> LoginWithEmailNotFound = () => { return Path.Combine(".", "Resources", "LoginWithEmailNotFound.json"); };
        public static Func<string> UpdateCustomerName = () => { return Path.Combine(".", "Resources", "UpdateCustomerName.json"); };
        public static Func<string> UpdateZipCode = () => { return Path.Combine(".", "Resources", "UpdateZipCode.json"); };
        public static Func<string> UpdateDob = () => { return Path.Combine(".", "Resources", "UpdateDob.json"); };
        public static Func<string> UpdateMailingAddress = () => { return Path.Combine(".", "Resources", "UpdateMailingAddress.json"); };
        public static Func<string> UpdateFinal = () => { return Path.Combine(".", "Resources", "UpdateFinal.json"); };
        public static Func<string> UpdateQuoteRentersInsurance = () => { return Path.Combine(".", "Resources", "UpdateQuoteRentersInsurance.json"); };
        public static Func<string> UpdateCustomerInformation = () => { return Path.Combine(".", "Resources", "UpdateQuoteRentersInsurance.json"); };
        public static Func<string> UpdateCoverageOptions = () => { return Path.Combine(".", "Resources", "UpdateCoverageOptions.json"); };
        public static Func<string> UpdateSendEmailToCustomer = () => { return ("UpdateSendEmail"); };
        public static Func<string> UpdatePolicySummary = () => { return Path.Combine(".", "Resources", "UpdatePolicySummary.json"); };
        public static Func<string> UpdateRenterInsurancePolicy = () => { return "UpdateRentersInsuranceCard"; };
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
                ExistingCustomer => LoginWithEmail(),
                LoginWithEmailCard => UpdateCustomerName(),
                LoginWithEmailNotFoundCard => UpdateCustomerName(),
                UpdateCustomerNameCard => UpdateZipCode(),
                UpdateZipCodeCard => UpdateDob(),
                UpdateDobCard => UpdateMailingAddress(),
                UpdateMailingAddressCard => UpdateFinal(),
                UpdateFinalCard => Card.UpdateQuoteCard,
                UpdateQuoteCard => Card.UpdateEmailConfirmationCard,
                UpdateEmailConfirmationOk => UpdateCoverageOptions(),
                UpdateCoverageCard => UpdatePolicySummary(),
                UpdatePolicySummaryCard => Card.UpdateRentersInsuranceCard,//ThumbNail - RentersInsurance Card
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
