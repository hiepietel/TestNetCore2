using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface ITempService
    {
        public Task<float> GetTemp();
        public Task<List<TemperatureHistory>> GetTempAll();
    }
}
