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
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetColor()
        {
            try
            {
                var response = await _colorService.GetColor(1);
                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [HttpPost]
        public async Task<IActionResult> SetColor()
        {
            try
            {
                var response = await _colorService.SetColor(1);
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


    }
}