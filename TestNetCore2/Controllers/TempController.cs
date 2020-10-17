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
        private readonly ITempService _tempService;
        public TempController(ITempService tempService)
        {
            _tempService = tempService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTemp()
        {
            try
            {
                var temp = await _tempService.GetTemp();
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
