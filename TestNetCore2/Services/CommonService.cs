using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class CommonService : ICommonService
    {
        public async Task<HttpClient> GetHttpClient(string uriAddress)
        {
            var uri = new Uri(ToUrl(uriAddress));
            var networkCredential = new NetworkCredential();
            var credentialsCache = new CredentialCache { { uri, "NTLM", networkCredential } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var client = new HttpClient(handler) { BaseAddress = uri };
            return client;
        }
        private string ToUrl(string url)
        {
            url = url.StartsWith("http://") ? url : "http://" + url;
            return url.EndsWith("/") ? url : url + "/";
        }
        //TODO - add StringContetToData Generic
        public async Task<T> StringContentToData<T>(HttpResponseMessage response)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            var toReturn = JsonConvert.DeserializeObject<T>(responseString);
            return toReturn;
        }

        public async Task<StringContent> DataToStringContent(object data)
        {
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
