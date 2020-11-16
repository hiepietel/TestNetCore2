using ClassLibrary.ArduinoData;
using ClassLibrary.Model;
using Microsoft.EntityFrameworkCore;
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
    public class ColorService : CommonService, IColorService
    {
        private readonly ApplicationContext dbContext;
        public ColorService(ApplicationContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ARGBB> GetColor(int deviceId)
        {
            var device = dbContext.Device.Single(x => x.Id == deviceId);
            HttpClient client = await GetHttpClient(device.Ip);
            var response = await client.GetAsync("rgb");
            var responseString = response.Content.ReadAsStringAsync().Result;
            var rgbb = JsonConvert.DeserializeObject<ARGBB>(responseString);
            return rgbb;
        }
        public async Task<List<Color>> GetAllColors()
        {
            List<Color> colorList = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .Select(c => (Color)c.GetValue(null, null))
                .ToList();
            return colorList;
        }
        public async Task<bool> SetColorToAll(ARGBB color)
        {
            var devices = await dbContext.Device.Where(x => x.Func == ClassLibrary.Enum.DeviceFunction.RGB).ToListAsync();
            if (devices.Count == 0) return false;
            var colorHistoryList = new List<ColorHistory>();
            foreach (var device in devices)
            {
                HttpClient client = await GetHttpClient(device.Ip);
                var result = await client.PostAsync("rgb", await DataToStringContent(color));
                if (result.IsSuccessStatusCode)
                {
                    var colorHistory = new ColorHistory()
                    {
                        DeviceId = device.Id,
                        Red = color.Red,
                        Green = color.Green,
                        Blue = color.Blue,
                        Brightness = color.Brightness,
                        Date = DateTime.Now
                    };
                    colorHistoryList.Add(colorHistory);
                }
            }
            await dbContext.AddRangeAsync(colorHistoryList);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SetColor(int deviceId, ARGBB color)
        {
            var device = dbContext.Device.Single(x => x.Id == deviceId);
            HttpClient client = await GetHttpClient(device.Ip);
            var result  = await client.PostAsync("rgb", await DataToStringContent(color));
            if (result.IsSuccessStatusCode)
            {
                var colorHistory = new ColorHistory()
                {
                    DeviceId = device.Id,
                    Red = color.Red,
                    Green = color.Green,
                    Blue = color.Blue,
                    Brightness = color.Brightness,
                    Date = DateTime.Now
                };
                await dbContext.ColorHistory.AddAsync(colorHistory);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ColorHistory>> GetColorHistory()
        {
            return await dbContext.ColorHistory.ToListAsync();
        }
    }
}
