using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class CommonService : ICommonService
    {
        public HttpClient GetHttpClient(string uriAddress)
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
    }
}
