using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ChatbotCustomerOnboarding;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using LaYumba.Functional;
using System.Threading.Tasks;
using System.Net.Http;

namespace KnowledgeBase
{
    using static F;

    public class QNAMaker : GenericHelpers
    {

        /*Default Answers*/
        const string defaultResponse = "No answers were found. Try different words or Type 'connect' to chat with an agent";
        const string kbDefaultResponse = "No good match found in KB.";
        private static Func<Task<string>> ReportError = async () => "An error occurred.";


        public static Func<ITurnContext<IMessageActivity>, Task<Activity>> KnowledgeBase = async (userString) =>
         {

             /* QNA Maker Connection */
             IAPIHelper Invoke = new APIHelper();
             string baseUrl = "https://rentersinsurancekb.azurewebsites.net/qnamaker";
             string uriPath = "/knowledgebases/fd89d3be-0316-42f8-afc5-bf2a13acc9f4/generateAnswer";
             string authorization = "2dec1a75-e103-4da4-8cc1-4cef36c4e9fa";


             /*Extract User Response*/
             Option<string> contents = ReadFileContent(Path.Combine(".", "KnowledgeBase", "QNAMakerRequest.json"));
             Option<string> getCustomerResponse = ModifyJson(contents.Match(Some: (e) => e, None: () => "None"), "question", userString.Activity.Text.ToString());
             if (getCustomerResponse == default) return MessageFactoryText(defaultResponse);


             /* Invoke QNA Maker*/
             /*Read and Parse QNA Maker Response*/
             Option<HttpResponseMessage> response = await Invoke.PostAPI(baseUrl, uriPath, HttpStatusCode.OK, getCustomerResponse.Match(Some: (e) => e, None: () => "None"), null, null, authorization);
             string jsonString = await response.Match(
                    None: () => ReportError(),
                    Some: async (r) => await r.Content.ReadAsStringAsync()
             );
             Root deserializedResponse = Deserialize<Root>(jsonString);
             var reply = IfValueExists(deserializedResponse) && IfValueExists(deserializedResponse.answers[0]) ? deserializedResponse.answers[0] : default;
             if (reply == default) return MessageFactoryText(defaultResponse);


             /*Followup Response*/
             // Followup Check Results
             var followUpCheckResult = Deserialize<FollowUpCheckResult>(jsonString);

             if (IfValueExists(deserializedResponse.answers) && IfValueExists(deserializedResponse.answers[0].context))
             {
                 var followReply = MessageFactory.Text(reply.answer);
                 // if follow-up check contains valid answer and at least one prompt, add prompt text to SuggestedActions using CardAction one by one
                 followReply.SuggestedActions = new SuggestedActions();
                 followReply.SuggestedActions.Actions = new List<CardAction>();
                 for (int i = 0; i < followUpCheckResult.Answers[0].Context.Prompts.Length; i++)
                 {
                     var promptText = followUpCheckResult.Answers[0].Context.Prompts[i].DisplayText;
                     followReply.SuggestedActions.Actions.Add(new CardAction() { Title = promptText, Type = ActionTypes.ImBack, Value = promptText });
                 }

                 followReply.Text = reply.answer;
                 return followReply;
             }


             /* Regular Response*/
             if (deserializedResponse.answers.Count == 1)
             {
                 return MessageFactoryText(reply.answer);
             }


             /* Default Response*/
             return (MessageFactoryText(defaultResponse));

         };

        /*Generic Message Text Factory*/
        public static Func<string, Microsoft.Bot.Schema.Activity> MessageFactoryText = (message) =>
        {
            return message.Equals(kbDefaultResponse) || message.Equals(defaultResponse) ? MessageFactory.Text(defaultResponse) : MessageFactory.Text(message);
        };
    }


    #region Regular Class Model
    public class Context
    {
        public bool isContextOnly { get; set; }
        public List<object> prompts { get; set; }
    }

    public class Answer
    {
        public List<string> questions { get; set; }
        public string answer { get; set; }
        public double score { get; set; }
        public int id { get; set; }
        public string source { get; set; }
        public bool isDocumentText { get; set; }
        public List<object> metadata { get; set; }
        public Context context { get; set; }
    }

    public class Root
    {
        public List<Answer> answers { get; set; }
        public bool activeLearningEnabled { get; set; }
    }
    #endregion

    #region Follow up Class Model
    class FollowUpCheckResult
    {
        [JsonProperty("answers")]
        public FollowUpCheckQnAAnswer[] Answers
        {
            get;
            set;
        }
    }

    class FollowUpCheckQnAAnswer
    {
        [JsonProperty("context")]
        public FollowUpCheckContext Context
        {
            get;
            set;
        }
    }

    class FollowUpCheckContext
    {
        [JsonProperty("prompts")]
        public FollowUpCheckPrompt[] Prompts
        {
            get;
            set;
        }
    }

    class FollowUpCheckPrompt
    {
        [JsonProperty("displayText")]
        public string DisplayText
        {
            get;
            set;
        }
    }
    #endregion

}