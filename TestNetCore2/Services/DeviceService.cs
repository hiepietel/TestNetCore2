using ClassLibrary;
using ClassLibrary.ArduinoData;
using ClassLibrary.DTO;
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
    public class DeviceService : CommonService, IDeviceService
    {
        ApplicationContext dbContext;
        public DeviceService(ApplicationContext _applicationContext)
        {
            dbContext = _applicationContext;
        }
        public async Task<List<DeviceInfoDTO>> GetAllDeviceInfo()
        {
            var deviceInfos = await dbContext.Device.ToListAsync();
            var deviceInfoDTOs = new List<DeviceInfoDTO>();
            foreach (var device in deviceInfos)
            {
                var deviceInfoDTO = new DeviceInfoDTO()
                {
                    Id = device.Id,
                    Description = device.Description,
                    Function = device.Func,
                    Name = device.Name,
                    IsAlive = false
                };
                try
                {
                    HttpClient client = await GetHttpClient(device.Ip);
                    var response = await client.GetAsync("/info");
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    var responseDevice = JsonConvert.DeserializeObject<ADeviceInfo>(responseString);
                    deviceInfoDTO.Ip = responseDevice.Ip;
                    deviceInfoDTO.IsAlive = true;
                    
                }
                catch (Exception ex)
                {

                    
                }
                deviceInfoDTOs.Add(deviceInfoDTO);
            }
            return deviceInfoDTOs;
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
