using ClassLibrary;
using ClassLibrary.ArduinoData;
using ClassLibrary.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class TestConnectionService : CommonService, ITestConnectionService
    {
        ApplicationContext dbContext;
        public TestConnectionService(ApplicationContext _applicationContext)
        {
            dbContext = _applicationContext;
        }
        public async Task<List<ADeviceInfo>> GetAllDeviceInfo()
        {
            var aDeviceInfos = new List<ADeviceInfo>();
            var deviceInfos = await dbContext.Device.ToListAsync();
            foreach (var device in deviceInfos)
            {
                try
                {
                    HttpClient client = await GetHttpClient(device.Ip);
                    var response = await client.GetAsync("/info");
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var responseDevice = JsonConvert.DeserializeObject<ADeviceInfo>(responseString);                 
                    aDeviceInfos.Add(responseDevice);
                }
                catch (Exception ex)
                {

                    
                }
            }
            return aDeviceInfos;
        }
        public async Task<Device> GetDeviceInfo(int deviceId)
        {
            var device = await dbContext.Device.SingleAsync(x => x.Id == deviceId);
            HttpClient client = await GetHttpClient(device.Ip);
            var response = await client.GetAsync("info");
            var responseString = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Device>(responseString);
        }
        public async Task<bool> SetDeviceInfo(int deviceId, ADeviceInfo aDeviceInfo){
            var device = await dbContext.Device.SingleAsync(x => x.Id == deviceId);
            HttpClient client = await GetHttpClient(device.Ip);
            var response = await client.PostAsync("info", await DataToStringContent(aDeviceInfo));
            return true;
        }


    }
}
