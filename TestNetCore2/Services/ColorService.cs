using ClassLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class ColorService : IColorService
    {
        public async Task<RGBB> GetColor(int deviceId)
        {
            HttpClient client = GetHttpClient("http://192.168.0.189/");
            var response = await client.GetAsync("rgb");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var rgbb = JsonConvert.DeserializeObject<RGBB>(responseString);
            return rgbb;
        }
        
        public Task<bool> SetColor(int deviceId)
        {
            HttpClient client = GetHttpClient("http://192.168.0.189/");
            client.PostAsync("rgb", )
        }

        private HttpClient GetHttpClient(string uriAddress)
        {
            var uri = new Uri(uriAddress);
            var networkCredential = new NetworkCredential();
            var credentialsCache = new CredentialCache { { uri, "NTLM", networkCredential } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var client = new HttpClient(handler) { BaseAddress = uri };
            return client;
        }
    }
}
