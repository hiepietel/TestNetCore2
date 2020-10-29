using ClassLibrary.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class CovidService : ICovidService
    {
        string link = "https://covid-19-data.p.rapidapi.com";
        
        public async Task<SingleCountry> GetCovidToday()
        {
            var client = new RestClient(link + "/country?format=json&name=poland");
            IRestResponse response = client.Execute(GetRestRequest(Method.GET));
            var obj = JsonConvert.DeserializeObject<List<SingleCountry>>(response.Content);
            return obj.FirstOrDefault();
        }


        private RestRequest GetRestRequest(RestSharp.Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("x-rapidapi-host", "covid-19-data.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "0b31f4685fmsh8f70d0744873f14p1242b7jsnf75aa4a45400");
            return request;
        }
    }
}
