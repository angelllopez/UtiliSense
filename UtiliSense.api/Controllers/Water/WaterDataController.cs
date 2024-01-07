using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UtiliSense.service.Contracts;

namespace UtiliSense.api.Controllers.Water
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterDataController : ControllerBase
    {
        private readonly IWaterDataService _service;
        public WaterDataController(IWaterDataService service)
        {
            _service = service;
        }

        public async Task<IActionResult> GetWaterDataAsync()
        {
            var data = await _service.GetAllWaterDataAsync();

            if (!data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}
