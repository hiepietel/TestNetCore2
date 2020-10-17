using ClassLibrary;
using ClassLibrary.ArduinoData;
using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface ITestConnectionService : IService
    {
        public Task<Device> GetDeviceInfo(int deviceId);
        public Task<bool> SetDeviceInfo(int deviceId, ADeviceInfo aDeviceInfo);
        public Task<List<ADeviceInfo>> GetAllDeviceInfo();

    }
}
