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
    public class TestConnectionController : Controller
    {
        private readonly ITestConnectionService _testConnectionService;
        public TestConnectionController(ITestConnectionService testConnectionService)
        {
            _testConnectionService = testConnectionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                await _testConnectionService.CheckDevice();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }

        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllDeviceInfo()
        {
            try
            {
                var ret = await _testConnectionService.GetAllDeviceInfo();
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetDeviceInfo()
        {
            try
            {
                var ret = await _testConnectionService.GetDeviceInfo();
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}