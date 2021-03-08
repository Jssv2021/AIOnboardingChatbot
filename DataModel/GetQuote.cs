using System;
namespace ChatbotCustomerOnboarding.DataModel
{
    public class GetQuote
    {
        public int ZipCode { get; set; }
        public String Quote { get; set; }
    }

    public static class Card
    {
        private static string quoteCard = "QuoteCard";
        private static string renterInsSummary = "RentersInsuranceSummary";
        private static string emailConfirmation = "EmailConfirmation";
        private static string adaptiveCard = "AdaptiveCard";
        private static string thumbnailCard = "ThumbNailCard";
        private static string quoteCardButton = "QuoteCardOk";
        private static string emailCardButton = "emailConfirmOk";

        public static string BotCardType { get; set; }

        public static string QuoteCard
        {
            get { return quoteCard; }
            set { quoteCard = value; }
        }

        public static string RentersInsuranceCard
        {
            get { return renterInsSummary; }
            set { renterInsSummary = value; }
        }
        public static string EmailConfirmationCard
        {
            get { return emailConfirmation; }
            set { emailConfirmation = value; }
        }
        public static string AdaptiveCard
        {
            get { return adaptiveCard; }
            set { adaptiveCard = value; }
        }
        public static string ThumbNailCard
        {
            get { return thumbnailCard; }
            set { thumbnailCard = value; }
        }
        public static string QuoteCardButton
        {
            get { return quoteCardButton; }
            set { quoteCardButton = value; }
        }
        public static string EmailCardButton
        {
            get { return emailCardButton; }
            set { emailCardButton = value; }
        }
    }
}
