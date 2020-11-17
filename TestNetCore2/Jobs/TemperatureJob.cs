using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Jobs
{
    public class TemperatureJob
    {
        private readonly ApplicationContext _dbContext;
        private readonly ITemperatureService _temperatureService;
        public TemperatureJob(ITemperatureService temperatureService, ApplicationContext dbContext)
        {
            _dbContext = dbContext;
            _temperatureService = temperatureService;
        }
        public async Task Execute()
        {

            try
            {
                await _temperatureService.AddNewTemperatureHistory();
            }
            catch ( Exception ex)
            {

            }
        }
    }
}
