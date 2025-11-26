using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtiliSense.api.Core.shared;
using UtiliSense.service.Contracts;
using UtiliSense.shared;
using UtiliSense.shared.DTOs;

namespace UtiliSense.api.Controllers.Gas;

/// <summary>
/// API controller for managing gas consumption data records.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GasDataController : ControllerBase
{
    private readonly IGasDataService _service;
    private readonly ILogger<GasDataController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GasDataController"/> class.
    /// </summary>
    /// <param name="service">The service used to retrieve gas data. This parameter cannot be null.</param>
    /// <param name="logger">The logger instance used for logging operations within the controller. This parameter cannot be null.</param>
    public GasDataController(IGasDataService service, ILogger<GasDataController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all available gas data records.
    /// </summary>
    /// <remarks>
    /// Returns a collection of gas data records. If no data is available, an empty collection is returned.
    /// If an internal server error occurs, a 500 status is returned with an error message.
    /// </remarks>
    /// <returns>
    /// An <see cref="ActionResult{IEnumerable{GasMeterReadingDto}}"/> with a 200 status code and the gas data collection if successful,
    /// or a 500 status code in case of an internal server error.
    /// </returns>
    /// <response code="200">Returns the collection of gas data records (may be empty).</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GasMeterReadingDto>>> GetAllGasDataAsync()
    {
        var result = await _service.GetAllGasDataAsync();
        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Retrieves gas data for a specific day.
    /// </summary>
    /// <remarks>
    /// Returns the gas data for the specified date.
    /// </remarks>
    /// <param name="date">The date for which to retrieve gas data.</param>
    /// <returns>
    /// An <see cref="ActionResult{GasDataDto}"/> containing the gas data for the specified day if successful,
    /// or a 400, 404, or 500 status code if an error occurs.
    /// </returns>
    /// <response code="200">Returns the gas data for the specified day.</response>
    /// <response code="400">If the specified date is out of range or invalid.</response>
    /// <response code="404">If no gas data is found for the specified date.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("by-day/{date:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GasMeterReadingDto>> GetGasDataByDay(DateTime date)
    {
        var result = await _service.GetGasDataByDayAsync(date);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.OutOfRange => BadRequest(),
                ErrorCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Retrieves gas data for the specified month.
    /// </summary>
    /// <remarks>
    /// Uses the provided <paramref name="date"/> to identify the month and year for which gas data should be retrieved.
    /// </remarks>
    /// <param name="date">The date representing the month and year for which to retrieve gas data.</param>
    /// <returns>
    /// An <see cref="ActionResult{IEnumerable{GasMeterReadingDto}}"/> containing the gas data for the specified month (may be empty),
    /// or a 400, 404, or 500 status code if an error occurs.
    /// </returns>
    /// <response code="200">Returns the gas data for the specified month.</response>
    /// <response code="400">If the specified date is out of range or invalid.</response>
    /// <response code="404">If no gas data is found for the specified month.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("by-month/{date:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GasMeterReadingDto>>> GetGasDataByMonth(DateTime date)
    {
        var result = await _service.GetGasDataByMonthAsync(date);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.OutOfRange => BadRequest(),
                ErrorCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Retrieves gas data for the specified year.
    /// </summary>
    /// <remarks>
    /// Uses the year component of the <paramref name="date"/> parameter to retrieve gas data.
    /// </remarks>
    /// <param name="date">The date representing the year for which gas data is requested.</param>
    /// <returns>
    /// An <see cref="ActionResult{IEnumerable{GasMeterReadingDto}}"/> containing the gas data for the specified year (may be empty),
    /// or a 400, 404, or 500 status code if an error occurs.
    /// </returns>
    /// <response code="200">Returns the gas data for the specified year.</response>
    /// <response code="400">If the specified date is out of range or invalid.</response>
    /// <response code="404">If no gas data is found for the specified year.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpGet("by-year/{date:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<GasMeterReadingDto>>> GetGasDataByYear(DateTime date)
    {
        var result = await _service.GetGasDataByYearAsync(date);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.OutOfRange => BadRequest(),
                ErrorCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new gas data record.
    /// </summary>
    /// <remarks>
    /// Processes the provided <paramref name="gasDataDto"/> and attempts to create a new record.
    /// </remarks>
    /// <param name="gasDataDto">The gas data record to be created.</param>
    /// <returns>
    /// An <see cref="ActionResult{GasDataDto}"/> indicating the result of the operation.
    /// </returns>
    /// <response code="201">If the gas data record was successfully created.</response>
    /// <response code="409">If a record for the specified date already exists.</response>
    /// <response code="400">If the provided gas data is invalid or out of range.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GasMeterReadingDto>> CreateGasDataRecord([FromBody] GasMeterReadingDto gasDataDto)
    {
        var result = await _service.CreateGasDataRecordAsync(gasDataDto);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.OutOfRange => BadRequest(),
                ErrorCode.Conflict => Conflict(ExternalErrorMessages.Conflict),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return CreatedAtAction(nameof(GetGasDataByDay), new { date = result.Data!.MeterReadDate }, result.Data);
    }

    /// <summary>
    /// Updates an existing gas data record in the system.
    /// </summary>
    /// <remarks>
    /// Processes the provided <paramref name="gasDataDto"/> and attempts to update the corresponding record.
    /// </remarks>
    /// <param name="gasDataDto">The gas data record to update.</param>
    /// <returns>
    /// A <see cref="Task{ActionResult}"/> representing the asynchronous operation. Returns <see cref="NoContentResult"/> if the update is successful.
    /// </returns>
    /// <response code="204">If the gas data record was successfully updated.</response>
    /// <response code="400">If the provided gas data is invalid or out of range.</response>
    /// <response code="404">If no gas data record is found for the specified identifier.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateGasDataRecord([FromBody] GasMeterReadingDto gasDataDto)
    {
        var result = await _service.UpdateGasDataRecordAsync(gasDataDto);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.OutOfRange => BadRequest(),
                ErrorCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a gas data record for the specified date.
    /// </summary>
    /// <remarks>
    /// Attempts to delete a gas data record for the given date.
    /// </remarks>
    /// <param name="date">The date of the gas data record to delete.</param>
    /// <returns>
    /// A <see cref="Task{ActionResult}"/> representing the asynchronous operation. Returns <see cref="NoContentResult"/> if the record is successfully deleted.
    /// </returns>
    /// <response code="204">If the gas data record was successfully deleted.</response>
    /// <response code="400">If the specified date is out of range or invalid.</response>
    /// <response code="404">If no gas data record is found for the specified date.</response>
    /// <response code="500">If an internal server error occurs.</response>
    [HttpDelete("by-day/{date:datetime}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteGasDataRecord(DateTime date)
    {
        Result<bool> result = await _service.DeleteGasDataRecordAsync(date);

        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                ErrorCode.NotFound => NotFound(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, ExternalErrorMessages.ServerError)
            };
        }

        return NoContent();
    }
}
