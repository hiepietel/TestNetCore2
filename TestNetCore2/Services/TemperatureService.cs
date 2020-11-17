using ClassLibrary.ArduinoData;
using ClassLibrary.DTO;
using ClassLibrary.Enum;
using ClassLibrary.Model;
using Microsoft.EntityFrameworkCore;
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
    public class TemperatureService : CommonService, ITemperatureService
    {
        private readonly ApplicationContext dbContext;
        public TemperatureService(ApplicationContext _dbContext)
        {
            dbContext = _dbContext;
        }

        //public async Task<List<TemperatureDTO>> GetTemperature()
        //{
        //    var tempDevice = await dbContext.Device.Where(x => x.Func == DeviceFunction.TEMP).ToListAsync();
        //    var client = await GetHttpClient(tempDevice.Ip);
        //    var response = await client.GetAsync("temp");
        //    var responseString = response.Content.ReadAsStringAsync().Result;
        //    var aTemp = JsonConvert.DeserializeObject<ATemp>(responseString);
        //    var temp = new TemperatureHistory()
        //    {
        //        DeviceId = tempDevice.Id,
        //        Temperature = aTemp.Temp,
        //        Date = DateTime.Now
        //    };
        //    await dbContext.TemperatureHistory.AddAsync(temp);
        //    await dbContext.SaveChangesAsync();
        //    return aTemp.Temp;
        //}
        public async Task<bool> AddNewTemperatureHistory()
        {
            var aTempList = await GetTemperature();
            var tempHistList = new List<TemperatureHistory>();
            foreach (var temp in aTempList)
            {
                var tempHist = new TemperatureHistory()
                {
                    Date = DateTime.Now,
                    Temperature = temp.Temperature,
                    DeviceId = temp.DeviceId
                };
                tempHistList.Add(tempHist);
            }
            await dbContext.AddRangeAsync(tempHistList);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<TemperatureHistory>> GetTemperatureHistory()
        {
            return await dbContext.TemperatureHistory.ToListAsync();
        }
        public async Task<List<TemperatureDTO>> GetTemperature()
        {
            var tempDevices = await dbContext.Device.Where(x => x.Func == DeviceFunction.TEMP).ToListAsync();
            var tempList = new List<TemperatureDTO>();
            foreach (var tempDevice in tempDevices)
            {
                try
                {
                    var client = await GetHttpClient(tempDevice.Ip);
                    var response = await client.GetAsync("temp");
                    var aTemp = await StringContentToData<ATemp>(response);
                    var temp = new TemperatureDTO()
                    {
                        Temperature = aTemp.Temp,
                        DeviceId = tempDevice.Id
                    };
                    tempList.Add(temp);
                }
                catch (Exception ex)
                {

                }
            }
            return tempList;

        }

    }
}
