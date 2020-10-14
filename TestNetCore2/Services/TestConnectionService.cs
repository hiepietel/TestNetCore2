using ClassLibrary;
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
    public class TestConnectionService : ITestConnectionService
    {
        public TestConnectionService()
        {

        }

        public async Task CheckDevice()
        {
            string uriAddress = "http://192.168.0.165/";
            //string login = await GetConfigValueAsync<string>("SSRSuser");
            //string password = await GetConfigValueAsync<string>("SSRSpass");

            var uri = new Uri(uriAddress);

            var networkCredential = new NetworkCredential();
            var credentialsCache = new CredentialCache { { uri, "NTLM", networkCredential } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var client = new HttpClient(handler) { BaseAddress = uri };
            var response = await client.GetAsync("");
            var responseString = response.Content.ReadAsStringAsync();
        }
        public async Task<Device> GetDeviceInfo()
        {
            HttpClient client = GetHttpClient("http://192.168.0.165/");
            var response = await client.GetAsync("info");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var device = JsonConvert.DeserializeObject<Device>(responseString);
            return device;
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
