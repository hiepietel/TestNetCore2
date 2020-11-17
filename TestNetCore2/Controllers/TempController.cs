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
    public class TempController : Controller
    {
        private readonly ITemperatureService _tempService;
        public TempController(ITemperatureService tempService)
        {
            _tempService = tempService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTemp()
        {
            try
            {
                var temp = await _tempService.GetTemperature();
                return Json(temp);
            }
            catch (Exception ex)
            {
                return Json(false);

            }
        }
        [HttpGet]
        [Route("history")]
        public async Task<IActionResult> GetTemperatureHistory()
        {
            try
            {
                var tempList = await _tempService.GetTemperatureHistory();
                return Json(tempList);
            }
            catch (Exception ex)
            {
                return Json(false);

            }
        }
    }
}
