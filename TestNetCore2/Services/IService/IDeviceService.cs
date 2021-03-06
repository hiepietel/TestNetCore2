﻿using ClassLibrary;
using ClassLibrary.ArduinoData;
using ClassLibrary.DTO;
using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface IDeviceService : IService
    {
        public Task<Device> GetDeviceInfo(int deviceId);
        public Task<bool> SetDeviceInfo(int deviceId, ADeviceInfo aDeviceInfo);
        public Task<List<DeviceInfoDTO>> GetAllDeviceInfo();

    }
}
