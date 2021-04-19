using System;
using System.IO;
using System.Net.Http;
using LaYumba.Functional;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static LaYumba.Functional.F;

namespace ChatbotCustomerOnboarding
{
    public class GenericHelpers
    {
        /*Generic utilites
       /*Generic Message Text Factory*/

        public static string NotExist = "NotExist";

        public static Option<string> ReadFileContent(string Path) { return File.Exists(Path) ? Some(File.ReadAllText(Path)) : None; }

        public static Option<string> ModifyJson(string contents, string propertyName, string properyValue)
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

        public static Func<JObject, string, bool> Exists = (jResult, property) => { return JObjectContainsKey(jResult, property).Match(Some: (e) => e, None: () => false); };

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
        public static T Deserialize<T>(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        /*Genric HShoul*/
        public static Func<dynamic, bool> ShouldbeLessThan = (responseObject) => { return (responseObject != null); };

        /*Convert Double*/

        public static Func<string, double> ConvertToDouble = (doubleString) =>
             {
                 var result = LaYumba.Functional.Double.Parse(doubleString);

                 return result.Match(Some: (e) => e, None: () => 0.0); ;
             };
    }
}
