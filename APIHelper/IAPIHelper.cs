using System;
using System.Net.Http;
using System.Threading.Tasks;
///-----------------------------------------------------------------
///   Namespace:      <ChatbotCustomerOnboarding>
///   Interface:          <IAPIHelper>
///   Description:    <API Interface for performing CRUD Operations - Chabot & Automation tests>
///   Author:         <Vignesh Chandran balan>                    
///   Notes:          <Notes>
///   Revision History:
///   Name:           Date:        Description:
///-----------------------------------------------------------------

namespace ChatbotCustomerOnboarding
{
    public interface IAPIHelper
    {
        public Task<HttpResponseMessage> GetAPI(string baseUrl, string uriPath, Enum responseCode, string tokenType = null, string ignoreError = null, string apimSubscriptionKey = null);
        public Task<HttpResponseMessage> PostAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null, string subscriptionName = null, string authorization = null);
        public Task<HttpResponseMessage> PatchAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null);
        public Task<HttpResponseMessage> PutAPI(string baseUrl, string uriPath, Enum responseCode, string payload, string apimSubscriptionKey = null);
    }
}
