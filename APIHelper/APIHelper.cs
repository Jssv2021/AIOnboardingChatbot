using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/* Generic REST API library to call customer onboarding services*/
///-----------------------------------------------------------------
///   Namespace:      <ChatbotCustomerOnboarding>
///   Class:          <APIHelper>
///   Description:    <API Library for performing CRUD Operations - Chabot & Automation tests>
///   Author:         <Vignesh Chandran balan>                    
///   Notes:          <Notes>
///   Revision History:
///   Name:           Date:        Description:
///-----------------------------------------------------------------

namespace ChatbotCustomerOnboarding
{
    public class APIHelper : IAPIHelper
    {
        public async Task<HttpResponseMessage> GetAPI(string baseUrl, string uriPath, Enum responseCode, string tokenType = null, string ignoreError = null, string apimSubscriptionKey = null)

        {
            try
            {
                var client = new HttpClient();

                var query = baseUrl + uriPath;

                string contentType = "application/json";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, query);

                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);

                var returnResponse = await client.SendAsync(httpRequestMessage);

                if (!(returnResponse.StatusCode.Equals(responseCode)))

                {
                    var message = $"Failed GetAPI Reponse Code for GET '{query}'. Expected: {responseCode}, Actual: " + $"{returnResponse.StatusCode}. Error Message: {returnResponse.Content.ReadAsStringAsync().Result}. Status: Fail";
                    //TODO Log
                }

                else

                {
                    //TODO Log
                }

                return returnResponse;
            }
            catch (Exception e)
            {
                //TODO Log
                return null;
            }
        }

        public async Task<HttpResponseMessage> PostAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null, string subscriptionName = null, string authorization = null)

        {
            try
            {
                var client = new HttpClient();

                var query = baseUrl + uriPath;

                string contentType = "application/json";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, query);

                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                httpRequestMessage.Headers.Add(subscriptionName ?? "Ocp-Apim-Subscription-Key", apimSubscriptionKey);

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("EndpointKey", authorization);
                //authorization ?? 

                httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (!(httpResponseMessage.StatusCode.Equals(responseCode)))

                {
                    var message = $"Failed POST Reponse Code '{query}'. Expected: {responseCode}, Actual: " + $"{httpResponseMessage.StatusCode}. Error Message: {httpResponseMessage.Content.ReadAsStringAsync().Result}. Status: Fail";

                    //TODO Log
                }

                else

                {
                    //TODO Log

                }

                return httpResponseMessage;
            }
            catch (Exception e)
            {
                //TODO Log
                return null;
            }
        }

        public async Task<HttpResponseMessage> PutAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null)

        {
            try
            {

                var client = new HttpClient();

                var query = baseUrl + uriPath;

                string contentType = "application/json";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, query);

                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);


                if (!(httpResponseMessage.StatusCode.Equals(responseCode)))

                {
                    var message = $"Failed PUT Reponse Code '{query}'. Expected: {responseCode}, Actual: " + $"{httpResponseMessage.StatusCode}. Error Message: {httpResponseMessage.Content.ReadAsStringAsync().Result}. Status: Fail";
                    //TODO Log
                }

                else

                {
                    //TODO Log
                }

                return httpResponseMessage;
            }
            catch (Exception e)
            {
                //TODO Log
                return null;
            }

        }

        public async Task<HttpResponseMessage> PatchAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null)

        {
            try
            {

                var client = new HttpClient();

                var query = baseUrl + uriPath;

                string contentType = "application/json";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, query);

                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));


                httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

                httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);

                httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);


                if (!(httpResponseMessage.StatusCode.Equals(responseCode)))

                {
                    var message = $"Failed PATCH Reponse Code '{query}'. Expected: {responseCode}, Actual: " + $"{httpResponseMessage.StatusCode}. Error Message: {httpResponseMessage.Content.ReadAsStringAsync().Result}. Status: Fail";
                    //TODO Log
                }

                else

                {
                    //TODO Log
                }

                return httpResponseMessage;
            }
            catch (Exception e)
            {
                //TODO Log
                return null;
            }

        }
    }
}
