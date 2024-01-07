using Microsoft.AspNetCore.Mvc;
using UtiliSense.service.Contracts;

namespace UtiliSense.api.Controllers.Gas
{
    [Route("api/[controller]")]
    [ApiController]
    public class GasDataController : ControllerBase
    {
        private readonly IGasDataService _service;
        public GasDataController(IGasDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGasDataAsync()
        {
            var data = await _service.GetAllGasDataAsync();

            if (!data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}
