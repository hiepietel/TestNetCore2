using ClassLibrary.ArduinoData;
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
    public class TempService : CommonService, ITempService
    {
        private readonly ApplicationContext dbContext;
        public TempService(ApplicationContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<float> GetTemp()
        {
            var tempDevice = await dbContext.Device.Where(x => x.Name == "TEMP").FirstOrDefaultAsync(); ;
            var client = GetHttpClient(tempDevice.Ip);
            var response = await client.GetAsync("temp");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var aTemp = JsonConvert.DeserializeObject<ATemp>(responseString);
            var temp = new TemperatureHistory()
            {
                DeviceId = tempDevice.Id,
                Temp = aTemp.Temp,
                Date = DateTime.Now
            };
            await dbContext.Temperature.AddAsync(temp);
            await dbContext.SaveChangesAsync();
            return aTemp.Temp;
        }

        public async Task<List<TemperatureHistory>> GetTempAll()
        {
            return await dbContext.Temperature.ToListAsync();
        }


    }
}
