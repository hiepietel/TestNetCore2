using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Services
{
    public class LCDService : ILCDService
    {
        public Task<bool> GetText()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetText()
        {
            throw new NotImplementedException();
        }
    }
}
