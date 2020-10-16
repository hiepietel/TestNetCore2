using ClassLibrary;
using ClassLibrary.ArduinoData;
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
        ApplicationContext DbContext;
        public TestConnectionService(ApplicationContext _applicationContext)
        {
            DbContext = _applicationContext;
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
        public async Task<List<ADeviceInfo>> GetAllDeviceInfo()
        {
            var Iplist = new List<string>();
            Iplist.Add("192.168.0.102");
            Iplist.Add("192.168.0.190");
            Iplist.Add("192.168.0.189");
            var deviceInfos = new List<ADeviceInfo>();
            foreach (string ip in Iplist)
            {
                try
                {
                    HttpClient client = GetHttpClient("http://" + ip);
                    var response = await client.GetAsync("/info");
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var device = JsonConvert.DeserializeObject<ADeviceInfo>(responseString);
                    deviceInfos.Add(device);
                }
                catch (Exception ex)
                {

                    
                }
            }
            return deviceInfos;
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
