using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChatbotCustomerOnboarding.BotHelpers;
using ChatbotCustomerOnboarding.DataModel;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/* Activity Handler/
   ///-----------------------------------------------------------------
   ///   Namespace:      <ChatbotCustomerOnboarding>
   ///   Class:          <QuoteCard>
   ///   Description:    <Message Activity Handler - Drives and handles the entire conversation flow>
   ///   Author:         <Vignesh Chandran balan>                    
 ///-----------------------------------------------------------------*/


namespace Microsoft.BotBuilderSamples.Bots
{
    public class ChatbotCustomerOnboard : ActivityHandler
    {

        private static readonly string _cards = Path.Combine(".", "Resources", "WelcomeCard.json");
        dynamic _cardAttachment;
        static dynamic _currentActiveCard;
        dynamic _quoteRate;
        static dynamic _userInput;
        static dynamic _userResponse;
        static bool _userResponseFlag;


        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            await SendWelcomeMessageAsync(turnContext, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                bool cardFlag = false;
                _userInput = turnContext.Activity.Value.ToString();
                if (UserResponse()) { _userResponse = _userInput; } else { _userResponse = JToken.Parse(_userInput); }
                dynamic cardType = DetectCardType();


                if (cardType == Card.AdaptiveCard)
                {
                    _currentActiveCard = ConversationalFlow.GetNextCard(_userResponse["button"].ToString()).Result.ToString();
                    AddUserInputs(turnContext);
                    if (_currentActiveCard == Card.QuoteCard) { GetCustomer.Quote = await QuoteCard.GetQuote(); _cardAttachment = CreateQuoteCardAttachment(); cardFlag = true; }
                    if (_currentActiveCard == Card.RentersInsuranceCard) { var policy = await QuoteCard.CreatePolicy(); _cardAttachment = CreateRentersInsuranceCardAttachment(); cardFlag = true; }
                    if (!cardFlag) _cardAttachment = CreateAdaptiveCardAttachment(_currentActiveCard);
                }

                else
                {
                    _currentActiveCard = ConversationalFlow.GetNextCard(_userResponse.ToString()).Result.ToString();
                    if (_currentActiveCard == Card.EmailConfirmationCard) { var customerInfo = await QuoteCard.CreateCustomer(); _cardAttachment = QuoteCardAttachment(); cardFlag = true; }
                    if (!cardFlag) _cardAttachment = CreateAdaptiveCardAttachment(_currentActiveCard);
                }

                await turnContext.SendActivityAsync(MessageFactory.Attachment(_cardAttachment), cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //ToDO add logger
            }
        }

        //Welcome Message Attachment

        private static async Task SendWelcomeMessageAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in turnContext.Activity.MembersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    var cardAttachment = CreateAdaptiveCardAttachment(_cards);

                    await turnContext.SendActivityAsync(MessageFactory.Attachment(cardAttachment), cancellationToken);
                }
            }
        }

        //Generic AdaptiveAttachment - For All adaptive cards

        private static Attachment CreateAdaptiveCardAttachment(string filePath)
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        //ThumbNail Attachments

        private static Attachment CreateQuoteCardAttachment()
        {
            var cardAttachment = QuoteCard.QuoteSummary();
            return cardAttachment;
        }
        private static Attachment CreateRentersInsuranceCardAttachment()
        {
            var coverage = QuoteCard.AddCoverage();

            var cardAttachment = QuoteCard.RentersInsuranceSummary();
            return cardAttachment;
        }


        //Identity CardType based on Response

        private static string DetectCardType()
        {
            if (_userResponse.ToString().Contains("{")) return Card.AdaptiveCard;
            return Card.ThumbNailCard;
        }

        //Add user input to data model

        private static void AddUserInputs(dynamic turnContext)
        {

            CustomerInfo.AddCustomerDetails(_userResponse);
        }

        public static bool UserResponse()
        {
            if (_userInput == Card.EmailCardButton || _userInput == Card.QuoteCardButton)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private static Attachment QuoteCardAttachment()
        {
            var cardAttachment = QuoteCard.EmailConfirmation();
            var emailFlag = EmailService.SendEmail(GetCustomer.Quote);
            return cardAttachment;
        }
    }
}

