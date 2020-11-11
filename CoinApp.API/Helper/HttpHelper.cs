using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CoinApp.API.Helper
{
    public class HttpHelper
    {
        public HttpClient Initial(string apiUrl)
        {
            var url = new HttpClient();
            url.BaseAddress = new Uri(apiUrl);
            return url;
        }
        public static async Task<HttpResponseMessage> Post<T>(string url, T contentValue, string token, string apiBasicUri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBasicUri);
                    var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var result = await client.PostAsync(url, content);
                    return result;
                }
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        public static async Task<HttpResponseMessage> Post<T>(string url, T contentValue, string apiBasicUri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiBasicUri);
                    var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(url, content);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<HttpResponseMessage> Put<T>(string url, T stringValue, string token, string apiBasicUri)
        {
            using (var client = new HttpClient())
            {
                var js = JsonConvert.SerializeObject(stringValue);
                client.BaseAddress = new Uri(apiBasicUri);
                var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync(url, content);
                return result;
            }
        }
        public static async Task<string> PutReject<T>(string url, T stringValue, string token, string apiBasicUri)
        {
            using (var client = new HttpClient())
            {
                var js = JsonConvert.SerializeObject(stringValue);
                client.BaseAddress = new Uri(apiBasicUri);
                var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync(url, content);
                var responseContent = result.Content.ReadAsStringAsync();
                var mess = responseContent.Result;
                return mess;
            }
        }
        public static async Task<string> PutMessage<T>(string url, T stringValue, string token, string apiBasicUri)
        {
            using (var client = new HttpClient())
            {
                var js = JsonConvert.SerializeObject(stringValue);
                client.BaseAddress = new Uri(apiBasicUri);
                var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.PutAsync(url, content);
                var responseContent = result.Content.ReadAsStringAsync();
                var mess = responseContent.Result;
                return mess;
            }
        }

        public static async Task<HttpResponseMessage> Get(string url, string token, string apiBasicUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBasicUri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(url);
                return result;
            }
        }
        public static async Task<HttpResponseMessage> GetWithLink(string url)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(url);
                return result;
            }
        }
        public static async Task<HttpResponseMessage> Delete(string url, string token, string apiBasicUri)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBasicUri);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.DeleteAsync(url);
                return result;
            }
        }


        public static HttpResponseMessage SendHttpWebRequest(string url, string method, string content = null)
        {
            using (var httpClient = new HttpClient())
            {
                var httpMethod = new HttpMethod(method);
                using (var httpRequestMessage = new HttpRequestMessage { RequestUri = new Uri(url), Method = httpMethod })
                {
                    if (httpMethod != HttpMethod.Get && content != null)
                    {
                        httpRequestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                    }
                    return httpClient.SendAsync(httpRequestMessage).Result;
                }
            }
        }
        public static string ReadWebResponse(HttpResponseMessage httpResponseMessage)
        {
            using (httpResponseMessage)
            {
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
        }

        internal static Task Get<T>(object p)
        {
            throw new NotImplementedException();
        }
    }
}
