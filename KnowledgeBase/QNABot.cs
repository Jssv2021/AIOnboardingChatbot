﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using ChatbotCustomerOnboarding;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LaYumba.Functional;
using static LaYumba.Functional.F;


namespace KnowledgeBase
{
    public class QNAMaker
    {

        /*Default Answers*/
        const string defaultResponse = "No answers were found. Try different words or Type 'connect' to chat with an agent";
        const string kbDefaultResponse = "No good match found in KB.";


        public static Func<ITurnContext<IMessageActivity>, dynamic> KnowledgeBase = (userString) =>
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
             var response = Invoke.PostAPI(baseUrl, uriPath, HttpStatusCode.OK, getCustomerResponse.Match(Some: (e) => e, None: () => "None"), null, null, authorization);
             var jsonString = ResultString(response.Result);
             Root deserializedResponse = Deserialize<Root>(jsonString.Match(Some: (e) => e, None: () => "None"));
             var reply = IfValueExists(deserializedResponse) && IfValueExists(deserializedResponse.answers[0]) ? deserializedResponse.answers[0] : default;
             if (reply == default) return MessageFactoryText(defaultResponse);


             /*Followup Response*/
             // Followup Check Results
             var followUpCheckResult = Deserialize<FollowUpCheckResult>(jsonString.Match(Some: (e) => e, None: () => "None"));

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

        /*Generic utilites
        /*Generic Message Text Factory*/

        public static Option<string> ReadFileContent(string Path) { return File.Exists(Path) ? Some(File.ReadAllText(Path)) : None; }

        static Option<string> ModifyJson(string contents, string propertyName, string properyValue)
        {

            var customerQuestion = JObjectParse(contents.ToString());

            var ifExists = JObjectContainsKey(customerQuestion, propertyName).Match(Some: (e) => e, None: () => false);

            if (ifExists)
            {
                customerQuestion[propertyName] = properyValue;

                return Some(customerQuestion.ToString());
            }
            else
            {
                return None;
            }
        }

        /*Generic ReadHttpResponse*/
        public static Option<string> ResultString(HttpResponseMessage response) { return response.Content.ReadAsStringAsync().Result.ToString(); }

        /*Generic Jobject Parse*/
        public static Func<string, JObject> JObjectParse = (jsoncontent) => { return JObject.Parse(jsoncontent); };

        public static Try<JObject> Parse(string s) => () => JObject.Parse(s);

        /*Generic Jobject Contains*/
        public static Option<bool> JObjectContainsKey(JObject responseObject, string key) { return responseObject.ContainsKey(key); }

        /*Generic IsNotNull*/
        public static Func<dynamic, bool> IfValueExists = (responseObject) => { return responseObject != null; };

        /*Generic Deserilization*/
        private static T Deserialize<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /*Genric HShoul*/
        static Func<dynamic, bool> ShouldbeLessThan = (responseObject) => { return (responseObject != null); };

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