using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using System.Text.RegularExpressions;

namespace ShopOfThings.Tests.Integration
{
    public class ApiBuilder
    {
        protected HttpClient _client;
        public ApiBuilder(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> PostRequest<T>(string link, object value) 
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await _client.PostAsync(link, new StringContent(json, null, "application/json"));
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<HttpResponseMessage> PostRequestWithoutDeserializing(string link, object value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await _client.PostAsync(link, new StringContent(json, null, "application/json"));
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }
        public async Task<T> GetRequest<T>(string link)
        {
            var response = await _client.GetAsync(link);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreNotEqual(response.StatusCode, HttpStatusCode.Unauthorized);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task LogIn(string nickName, string password) 
        {
            var loginModel = new 
            {
                nickName = nickName,
                password = password
            };

            var token = await PostRequest<TokenModel>("api/login", loginModel);
            string bearerAuth = "Bearer " + token.Token;
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", bearerAuth);
            var a = 
        }

    }
}
