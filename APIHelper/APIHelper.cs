using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;


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
        public async Task<HttpResponseMessage> GetAPI(string baseUrl, string uriPath, System.Enum responseCode, string tokenType = null, string ignoreError = null, string apimSubscriptionKey = null)

        {
            var client = new HttpClient();

            var query = baseUrl + uriPath;

            string contentType = "application/json";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, query);

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);

            Option<HttpResponseMessage> returnResponse = await client.SendAsync(httpRequestMessage);

            return returnResponse.Match<HttpResponseMessage>(Some: (s) => s, None: null);
        }

        public async Task<HttpResponseMessage> PostAPI(string baseUrl, string uriPath, System.Enum responseCode, string payload, string apimSubscriptionKey = null, string subscriptionName = null, string authorization = null)

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

            Option<HttpResponseMessage> httpResponseMessage = await client.SendAsync(httpRequestMessage);

            return httpResponseMessage.Match<HttpResponseMessage>(Some: (s) => s, None: null);

        }

        public async Task<HttpResponseMessage> PutAPI(string baseUrl, string uriPath, System.Enum responseCode, string payload, string apimSubscriptionKey = null)

        {
            var client = new HttpClient();

            var query = baseUrl + uriPath;

            string contentType = "application/json";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, query);

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

            Option<HttpResponseMessage> httpResponseMessage = await client.SendAsync(httpRequestMessage);

            return httpResponseMessage.Match<HttpResponseMessage>(Some: (s) => s, None: null);

        }

        public async Task<HttpResponseMessage> PatchAPI(string baseUrl, string uriPath, System.Enum responseCode, string payload, string apimSubscriptionKey = null)

        {

            var client = new HttpClient();

            var query = baseUrl + uriPath;

            string contentType = "application/json";

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, query);

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));


            httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

            httpRequestMessage.Headers.Add("Ocp-Apim-Subscription-Key", apimSubscriptionKey);

            httpRequestMessage.Content = (HttpContent)new StringContent(payload, Encoding.UTF8, contentType);

            Option<HttpResponseMessage> httpResponseMessage = await client.SendAsync(httpRequestMessage);

            return httpResponseMessage.Match<HttpResponseMessage>(Some: (s) => s, None: null);
        }


        public static IActionResult Ok() => new OkResult();
        public static IActionResult Ok(object value) => new OkObjectResult(value);
        public static IActionResult BadRequest(object error) => new BadRequestObjectResult(error);
        public static IActionResult InternalServerError(object value)
        {
            var result = new ObjectResult(value);
            result.StatusCode = 500;
            return result;
        }
        public static IActionResult NotFound(object value)
        {
            var result = new ObjectResult(value);
            result.StatusCode = 404;
            return result;
        }
    }
}
