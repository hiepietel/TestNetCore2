using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary.ArduinoData;
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
        [Route("{DeviceId}")]
        public async Task<IActionResult> GetDeviceInfo(int DeviceId)
        {
            try
            {
                var ret = await _testConnectionService.GetDeviceInfo(DeviceId);
                return Json(ret);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpPost]
        [Route("{DeviceId}")]
        public async Task<IActionResult> SetDeviceInfo(int DeviceId, ADeviceInfo aDeviceInfo)
        {
            try
            {
                await _testConnectionService.SetDeviceInfo(DeviceId, aDeviceInfo);
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}