using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface IColorService : IService
    {
        public Task<bool> SetColor(int deviceId);
        public Task<DeviceRGB> GetColor(int deviceId);
        public Task<List<Color>> GetAllColors();

    }
}
