using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestNetCore2.Services.IService;

namespace TestNetCore2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CovidController : Controller
    {
        private readonly ICovidService _covidService;
        public CovidController(ICovidService covidService)
        {
            _covidService = covidService;
        }
        [HttpGet]
        [Route("today")]
        public async Task<IActionResult> GetAllDeviceInfo()
        {
            try
            {
                var ret = await _covidService.GetCovidToday();
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}
