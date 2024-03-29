﻿using Microsoft.AspNetCore.Mvc;
using UtiliSense.service.Contracts;

namespace UtiliSense.api.Controllers.Solar
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolarDataController : ControllerBase
    {
        private readonly ISolarDataService _service;
        public SolarDataController(ISolarDataService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSolarDataAsync()
        {
            var data = await _service.GetAllSolarDataAsync();

            if (!data.Any())
            {
                return NotFound();
            }

            return Ok(data);
        }
    }
}
