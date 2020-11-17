using ClassLibrary.DTO;
using ClassLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface ITemperatureService
    {
        public Task<List<TemperatureDTO>> GetTemperature();
        public Task<List<TemperatureHistory>> GetTemperatureHistory();
        public Task<bool> AddNewTemperatureHistory();
    }
}
