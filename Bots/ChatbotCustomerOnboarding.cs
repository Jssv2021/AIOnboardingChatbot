using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards.Templating;
using ChatbotCustomerOnboarding.BotHelpers;
using ChatbotCustomerOnboarding.DataModel;
using KnowledgeBase;
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

                //Call to Knowledge Base
                if (turnContext.Activity.TextFormat != null)
                {
                    var userResponse = QNAMaker.KnowledgeBase(turnContext);
                    await turnContext.SendActivityAsync(userResponse, cancellationToken);
                    return;
                };


                _userInput = UserInputType(turnContext);
                if (UserResponse()) { _userResponse = _userInput; } else { _userResponse = JToken.Parse(_userInput); }
                dynamic cardType = DetectCardType();


                if (cardType == Card.AdaptiveCard)
                {
                    _currentActiveCard = ConversationalFlow.GetNextCard(_userResponse["button"].ToString()).Result.ToString();
                    if (_currentActiveCard.Contains("Update"))
                    {
                        string customerEmail = _userResponse["emailAddress"];
                        JToken response = (JToken)_userResponse;
                        CustomerDto customerDto = (response.Count() > 2) ? JsonConvert.DeserializeObject<CustomerDto>(JsonConvert.SerializeObject(_userResponse)) : await CustomerInfo.GetCustomer(customerEmail);
                        CustomerDto nextCardDto = await CustomerInfo.GetCustomer(customerEmail);
                        _cardAttachment = CreateAdaptiveCardAttachmentDto(nextCardDto, _currentActiveCard);
                        UpdateUser(customerDto);
                        if (_currentActiveCard.Contains("Final")) await CustomerInfo.UpdateAndSave(customerDto.customerId);
                        cardFlag = true;
                    }
                    else
                    {
                        AddUserInputs(_userResponse);
                    }
                    if (_currentActiveCard == Card.QuoteCard) { GetCustomer.Instance.Quote = await QuoteCard.GetQuote(); _cardAttachment = CreateQuoteCardAttachment(); cardFlag = true; }
                    if (_currentActiveCard == Card.RentersInsuranceCard) { var policy = await QuoteCard.CreatePolicy(); _cardAttachment = CreateRentersInsuranceCardAttachment(); cardFlag = true; }
                    if (!cardFlag) _cardAttachment = CreateAdaptiveCardAttachment(_currentActiveCard);
                }
                else
                {
                    _currentActiveCard = ConversationalFlow.GetNextCard(_userResponse.ToString()).Result.ToString();
                    if (_currentActiveCard == Card.EmailConfirmationCard) { var customerInfo = await QuoteCard.CreateCustomerRentersInsurance(); _cardAttachment = QuoteCardAttachment(); cardFlag = true; }
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

        private static Func<string, Attachment> CreateAdaptiveCardAttachment = (filePath) =>
        {
            var adaptiveCardJson = File.ReadAllText(filePath);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        };

        //Generic AdaptiveAttachment - For All adaptive cards

        private static Func<CustomerDto, string, Attachment> CreateAdaptiveCardAttachmentDto = (customer, filePath) =>
        {
            var customerInformationAsJsonString = JsonConvert.SerializeObject(customer);
            var adaptiveCardJson = File.ReadAllText(filePath);
            AdaptiveCardTemplate template = new AdaptiveCardTemplate(adaptiveCardJson);
            string cardJson = template.Expand(customer);
            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(cardJson),
            };
            return adaptiveCardAttachment;
        };

        //ThumbNail Attachments

        private static Func<Attachment> CreateQuoteCardAttachment = () =>
      {
          var cardAttachment = QuoteCard.QuoteSummary();
          return cardAttachment;
      };

        private static Func<Attachment> CreateRentersInsuranceCardAttachment = () =>
        {
            var coverage = QuoteCard.AddCoverage();
            var cardAttachment = QuoteCard.RentersInsuranceSummary();
            return cardAttachment;
        };

        //Identity CardType based on Response

        private static Func<string> DetectCardType = () =>
        {
            if (_userResponse.ToString().Contains("{")) return Card.AdaptiveCard;
            return Card.ThumbNailCard;
        };

        //Add user input to data model

        private static Action<dynamic> AddUserInputs = (_userResponse) => { CustomerInfo.AddCustomerDetails(_userResponse); };

        //Edit user information

        private static async Task UpdateUser(CustomerDto customerDto) 
        {
            CustomerInfo.UpdateCustomerDetails(customerDto);
            if (_currentActiveCard.Contains("Final")) await CustomerInfo.UpdateAndSave(customerDto.customerId);
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

        private static Func<Attachment> QuoteCardAttachment = () =>
            {
                var cardAttachment = QuoteCard.EmailConfirmation();
                var emailFlag = EmailService.SendEmail(GetCustomer.Instance.Quote);
                return cardAttachment;
            };

        private static Func<dynamic, string> UserInputType = (userContextResponse) =>
        {
            if (userContextResponse.Activity.Text == null)
            {
                return userContextResponse.Activity.Value.ToString();
            }
            else
            {
                return userContextResponse.Activity.Text.ToString();
            }
        };
    }
}

