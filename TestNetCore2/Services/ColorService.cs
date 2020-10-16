using ClassLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class ColorService : IColorService
    {
        public async Task<DeviceRGB> GetColor(int deviceId)
        {
            HttpClient client = GetHttpClient("http://192.168.0.189/");
            var response = await client.GetAsync("rgb");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var rgbb = JsonConvert.DeserializeObject<DeviceRGB>(responseString);
            return rgbb;
        }
        public async Task<List<Color>> GetAllColors()
        {
            List<Color> colorList = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .Select(c => (Color)c.GetValue(null, null))
                .ToList();
            return colorList;
        }
        public async Task<bool> SetColor(int deviceId)
        {
            HttpClient client = GetHttpClient("http://192.168.0.189/");
            var data = new DeviceRGB
            {
                Red = 255, Blue = 255, Green= 255, Brightness =255
            };
            var dataAsString = JsonConvert.SerializeObject(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            await client.PostAsync("rgb", content);
            return true;
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
