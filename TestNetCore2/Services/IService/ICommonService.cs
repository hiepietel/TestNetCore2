using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TestNetCore2.Services.IService
{
    public interface ICommonService : IService
    {
        public Task<HttpClient> GetHttpClient(string uriAddress);
        public Task<StringContent> DataToStringContent(object data);
        public Task<T> StringContentToData<T>(HttpResponseMessage response);
    }
}
