using System;
using System.Collections.Generic;
using System.Net;
using ChatbotCustomerOnboarding;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnowledgeBase
{
    public class QNAMaker
    {
        public static Func<ITurnContext<IMessageActivity>, string> KnowledgeBase = (userString) =>
         {
             try
             {
                 IAPIHelper Invoke = new APIHelper();

                 string baseUrl = "https://rentersinsurancekb.azurewebsites.net/qnamaker";

                 string uriPath = "/knowledgebases/fd89d3be-0316-42f8-afc5-bf2a13acc9f4/generateAnswer";

                 string payload = "{\"question\":\"" + userString.Activity.Text.ToString() + "}\"}";

                 string authorization = "2dec1a75-e103-4da4-8cc1-4cef36c4e9fa";

                 var response = Invoke.PostAPI(baseUrl, uriPath, HttpStatusCode.OK, payload, null, null, authorization);

                 var jsonString = response.Result.Content.ReadAsStringAsync().Result.ToString();

                 Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);

                 return myDeserializedClass.answers[0].answer.ToString();
             }

             catch (Exception e)
             {
                 return "Assitant doesn't have good answer; Please reach Agent for help";
             }
         };
    }

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
}