using Microsoft.AspNetCore.Mvc;
using UtiliSense.service.Contracts;

namespace UtiliSense.api.Controllers.Grid
{
    [Route("api/[controller]")]
    [ApiController]
    public class GridDataController : ControllerBase
    {
        private readonly IGridDataService _service;
        public GridDataController(IGridDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGridDataAsync()
        {
            var data = await _service.GetAllGridDataAsync();

            if (!data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}
