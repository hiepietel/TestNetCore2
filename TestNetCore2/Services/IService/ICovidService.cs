using ClassLibrary.Model;

using System.Threading.Tasks;

namespace TestNetCore2.Services.IService
{
    public interface ICovidService
    {
        public Task<SingleCountry> GetCovidToday();
    }
}
