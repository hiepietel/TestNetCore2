using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface ILCDService : IService
    {
        public Task<bool> SetText();
        public Task<bool> GetText();
    }
}
