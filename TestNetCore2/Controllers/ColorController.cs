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
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        [Route("{deviceId}")]
        public async Task<IActionResult> GetColor(int deviceId)
        {
            try
            {
                var response = await _colorService.GetColor(deviceId);
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpPost]
        [Route("{deviceId}")]
        public async Task<IActionResult> SetColor([FromBody] ARGBB color,int deviceId)
        {
            try
            {
                var response = await _colorService.SetColor(deviceId, color);
                return Json(response);
            }
            catch (Exception ex)
            {
                 return Json(false);
            }
        }


        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllColors()
        {
            try
            {
                return Json(await _colorService.GetAllColors());
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpGet]
        [Route("history")]
        public async Task<IActionResult> GetColorHistory()
        {
            try
            {
                return Json(await _colorService.GetColorHistory());
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

    }
}