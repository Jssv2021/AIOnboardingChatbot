using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChatbotCustomerOnboarding.KnowledgeBase
{
    public class FeedbackRecords
    {
        // <summary>
        /// List of feedback records
        /// </summary>
        [JsonProperty("feedbackRecords")]
        public FeedbackRecord[] Records { get; set; }
    }

    /// <summary>
    /// Active learning feedback record
    /// </summary>
    public class FeedbackRecord
    {
        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User question
        /// </summary>
        public string UserQuestion { get; set; }

        /// <summary>
        /// QnA Id
        /// </summary>
        public int QnaId { get; set; }
    }


    public class Train
    {

        /// <summary>
        /// Method to call REST-based QnAMaker Train API for Active Learning
        /// </summary>
        /// <param name="endpoint">Endpoint URI of the runtime</param>
        /// <param name="FeedbackRecords">Feedback records train API</param>
        /// <param name="kbId">Knowledgebase Id</param>
        /// <param name="key">Endpoint key</param>
        /// <param name="cancellationToken"> Cancellation token</param>
        public async static void CallTrain(string endpoint, FeedbackRecords feedbackRecords, string kbId, string key, CancellationToken cancellationToken)
        {
            var uri = endpoint + "/knowledgebases/" + kbId + "/train/";

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(uri);
                    request.Content = new StringContent(JsonConvert.SerializeObject(feedbackRecords), Encoding.UTF8, "application/json");
                    request.Headers.Add("Authorization", "EndpointKey " + key);

                    JObject customerQuestion = JObject.Parse(File.ReadAllText(Path.Combine(".", "KnowledgeBase", "FeedbackRecord.json")));

                    var response = await client.SendAsync(request, cancellationToken);
                    await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
