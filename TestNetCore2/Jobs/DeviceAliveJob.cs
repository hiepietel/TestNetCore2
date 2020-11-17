using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Jobs
{
    public class DeviceAliveJob
    {
        private readonly ApplicationContext _dbContext;
        private readonly IDeviceService _deviceService;
        public DeviceAliveJob(IDeviceService deviceService, ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _deviceService = deviceService;

        }
        public async Task Execute()
        {
            var deviceList = await _deviceService.GetAllDeviceInfo();
            var deviceHistoryList = new List<DeviceHistory>();
            foreach (var device in deviceList)
            {
                var deviceHistory = new DeviceHistory()
                {
                    Ip = device.Ip,
                    IsAlive = device.IsAlive,
                    Date = DateTime.Now
                };
                deviceHistoryList.Add(deviceHistory);
            }
            await _dbContext.AddRangeAsync(deviceHistoryList);
            await _dbContext.SaveChangesAsync();

        }

    }
}
