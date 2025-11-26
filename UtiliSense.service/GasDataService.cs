using UtiliSense.api.Core.shared;
using UtiliSense.data;
using UtiliSense.data.Models;
using UtiliSense.service.Contracts;
using UtiliSense.shared.DTOs;
using UtiliSense.shared;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace UtiliSense.service
{
    public class GasDataService(UtiliSenseDbContext db, ILogger<GasDataService> logger, IMapper mapper, IValidator<GasMeterReadingDto> validator) : IGasDataService
    {
        private readonly UtiliSenseDbContext _db = db ?? throw new ArgumentNullException(nameof(db));
        private readonly IValidator<GasMeterReadingDto> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        private readonly ILogger<GasDataService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<Result<GasMeterReadingDto>> CreateGasDataRecordAsync(GasMeterReadingDto gasMeterReadingDto)
        {
            try
            {
                #region validation
                if (gasMeterReadingDto == null)
                {
                    var result = Result<GasMeterReadingDto>.Failure(
                        ErrorCode.NullOrEmpty,
                        InternalErrorMessages.NullOrEmptyParameter(nameof(gasMeterReadingDto)));
                    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage ?? "Unknown validation error");
                    return result;
                }

                var validationResult = _validator.Validate(gasMeterReadingDto);
                if (!validationResult.IsValid)
                {
                    var result = Result<GasMeterReadingDto>.Failure(
                        ErrorCode.ValidationError,
                        InternalErrorMessages.ValidationFailed(nameof(GasMeterReadingDto), validationResult.Errors));
                    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage ?? "Unknow validation error");
                    return result;
                }

                // validate required fields
                //if (gasMeterReadingDto.MeterReadDate == default)
                //{
                //    var result = Result<GasMeterReadingDto>.Failure(
                //        ErrorCode.ValidationError,
                //        InternalErrorMessages.NullOrEmptyParameter(nameof(gasMeterReadingDto.MeterReadDate)));
                //    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage);
                //    return result;
                //}

                // validate non-negative values
                //if (gasMeterReadingDto.Consumption < 0 || gasMeterReadingDto.Cost < 0)
                //{
                //    var result = Result<GasMeterReadingDto>.Failure(
                //        ErrorCode.ValidationError,
                //        InternalErrorMessages.InvalidValue("Consumption or Cost", "Negative value"));
                //    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage);
                //    return result;
                //}

                // validate date is not in the future   
                if (gasMeterReadingDto.MeterReadDate > DateTime.Now)
                {
                    var result = Result<GasMeterReadingDto>.Failure(
                        ErrorCode.OutOfRange,
                        InternalErrorMessages.InvalidValue(nameof(gasMeterReadingDto.MeterReadDate), gasMeterReadingDto.MeterReadDate));
                    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage ?? "Unknown validation error");
                    return result;
                }

                // Check for existing record with the same BillingMonth
                var existingRecord = await _db.GasMeterReadings.FirstOrDefaultAsync(g => g.MeterReadDate == gasMeterReadingDto.MeterReadDate);
                if (existingRecord != null)
                {
                    var result = Result<GasMeterReadingDto>.Failure(
                        ErrorCode.Conflict,
                        InternalErrorMessages.RecordConflict(
                            nameof(GasMeterReading),
                            gasMeterReadingDto.MeterReadDate.ToString() 
                        ));
                    LogValidationFailure(nameof(CreateGasDataRecordAsync), result.ErrorMessage ?? "Unknown validation error");
                    return result;
                }
                #endregion
                var entity = _mapper.Map<GasMeterReading>(gasMeterReadingDto);
                await _db.GasMeterReadings.AddAsync(entity);
                await _db.SaveChangesAsync();

                var resultDto = _mapper.Map<GasMeterReadingDto>(entity);
                return Result<GasMeterReadingDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                string errorMessage = ex switch
                {
                    AutoMapperMappingException amEx => InternalErrorMessages.AutomapperMappingException(
                        nameof(GasMeterReadingDto),
                        nameof(GasMeterReading),
                        amEx.Message),
                    ArgumentNullException argNullEx => InternalErrorMessages.ArgumentNullException(
                        argNullEx.Message
                        ?? "unknown"),
                    _ => $"An unexpected error occurred: {ex.Message}"
                };

                _logger.LogError(ex, "{method}: Exception occurred: {errorMessage}", nameof(CreateGasDataRecordAsync), errorMessage);
                return Result<GasMeterReadingDto>.Failure(ErrorCode.InternalServerError, errorMessage);
            }
        }

        public async Task<Result<IEnumerable<GasMeterReadingDto>>> GetAllGasDataAsync()
        {
            try
            {
                _logger.LogInformation("GetAllGasDataAsync: Retrieving all gas data records.");
                var dataRecords = await _db.GasMeterReadings.ToListAsync();
                if (!dataRecords.Any())
                {
                    var result = Result<IEnumerable<GasMeterReadingDto>>.Failure(
                        ErrorCode.NotFound,
                        InternalErrorMessages.RecordNotFound(nameof(GasMeterReading), "all")
                    );
                    _logger.LogError("GetAllGasDataAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                var gasDataDtos = _mapper.Map<List<GasMeterReadingDto>>(dataRecords);
                return Result<IEnumerable<GasMeterReadingDto>>.Success(gasDataDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllGasDataAsync: Exception occurred while retrieving gas data records: {ex.Message}");
                return Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.InternalServerError, "An error occurred while retrieving gas data records.");
            }
        }

        public async Task<Result<GasMeterReadingDto>> GetGasDataByDayAsync(DateTime date)
        {
            try
            {
                // TODO: Need to decide if we want to validate the date input using a range or just check for no future dates.
                // Validate the input date
                //if (IsDateOutOfRange(date))
                //{
                //    var result = Result<GasMeterReadingDto>.Failure(ErrorCode.OutOfRange, InternalErrorMessages.OutOfRangeValue(nameof(date), date));
                //    _logger.LogError("GetGasDataByDayAsync: {Result}", result.ErrorMessage);
                //    return result;
                //}

                _logger.LogInformation("GetGasDataByDayAsync: Retrieves gas consumption data for a specific day.");
                var gasDataRecord = await _db.GasMeterReadings.FirstOrDefaultAsync(g => g.MeterReadDate.Date == date.Date);
                if (gasDataRecord == null)
                {
                    var result = Result<GasMeterReadingDto>.Failure(ErrorCode.NotFound, $"No gas data found for {date.Date}.");
                    _logger.LogError("GetGasDataByDayAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                var gasDataDto = _mapper.Map<GasMeterReadingDto>(gasDataRecord);
                return Result<GasMeterReadingDto>.Success(gasDataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetGasDataByDayAsync: Exception occurred while retrieving gas data for {date.Date}: {ex.Message}");
                return Result<GasMeterReadingDto>.Failure(ErrorCode.InternalServerError, "An error occurred while retrieving gas data.");
            }
        }

        public async Task<Result<IEnumerable<GasMeterReadingDto>>> GetGasDataByMonthAsync(DateTime date)
        {
            try
            {
                _logger.LogInformation("GetGasDataByMonthAsync: Retrieves gas data from the specified month.");
                var gasDataRecords = await _db.GasMeterReadings.Where(g => g.MeterReadDate.Month == date.Month && g.MeterReadDate.Year == date.Year).ToListAsync();
                if (!gasDataRecords.Any())
                {
                    var result = Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.NotFound, $"No gas data found for {date:MMMM yyyy}.");
                    _logger.LogError("GetGasDataByMonthAsync: {Result}",
                                     result.ErrorMessage);
                    return result;
                }

                var gasDataDtos = _mapper.Map<List<GasMeterReadingDto>>(gasDataRecords);
                return Result<IEnumerable<GasMeterReadingDto>>.Success(gasDataDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetGasDataByMonthAsync: Exception occurred while retrieving gas data for {date:MMMM yyyy}: {ex.Message}");
                return Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.InternalServerError, "An error occurred while retrieving gas data.");
            }
        }

        public async Task<Result<IEnumerable<GasMeterReadingDto>>> GetGasDataByYearAsync(DateTime date)
        {
            try
            {
                _logger.LogInformation("GetGasDataByYearAsync: Retrieves gas data for the specified year.");

                if (date == default)
                {
                    var result = Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.NullOrEmpty, InternalErrorMessages.NullOrEmptyParameter(nameof(date)));
                    _logger.LogError("GetGasDataByYearAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                //if (IsDateOutOfRange(date))
                //{
                //    var result = Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.OutOfRange, InternalErrorMessages.OutOfRangeValue(nameof(date), date));
                //    return result;
                //}

                var gasDataRecords = await _db.GasMeterReadings
                    .Where(g => g.MeterReadDate.Year == date.Year)
                    .ToListAsync();

                if (!gasDataRecords.Any())
                {
                    var result = Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.NotFound, $"No gas data found for the year {date.Year}.");
                    _logger.LogError("GetGasDataByYearAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                var gasDataDtos = _mapper.Map<List<GasMeterReadingDto>>(gasDataRecords);
                return Result<IEnumerable<GasMeterReadingDto>>.Success(gasDataDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGasDataByYearAsync: Exception occurred while retrieving gas data for the year {Year}", date.Year);
                return Result<IEnumerable<GasMeterReadingDto>>.Failure(ErrorCode.InternalServerError, "An error occurred while retrieving gas data.");
            }
        }

        public async Task<Result<bool>> UpdateGasDataRecordAsync(GasMeterReadingDto gasDataDto)
        {
            try
            {
                if (gasDataDto == null)
                {
                    var result = Result<bool>.Failure(
                        ErrorCode.NullOrEmpty,
                        InternalErrorMessages.NullOrEmptyParameter(nameof(gasDataDto)));
                    _logger.LogError("UpdateGasDataRecordAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                if (gasDataDto.MeterReadDate == default)
                {
                    var result = Result<bool>.Failure(
                        ErrorCode.ValidationError,
                        InternalErrorMessages.NullOrEmptyParameter(nameof(gasDataDto.MeterReadDate)));
                    _logger.LogError("UpdateGasDataRecordAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                //if (gasDataDto.Consumption < 0 || gasDataDto.Cost < 0)
                //{
                //    var result = Result<bool>.Failure(
                //        ErrorCode.ValidationError,
                //        InternalErrorMessages.InvalidValue("Consumption or Cost", "Negative value"));
                //    _logger.LogError("UpdateGasDataRecordAsync: {Result}", result.ErrorMessage);
                //    return result;
                //}

                if (gasDataDto.MeterReadDate > DateTime.Now)
                {
                    var result = Result<bool>.Failure(
                        ErrorCode.OutOfRange,
                        InternalErrorMessages.InvalidValue(nameof(gasDataDto.MeterReadDate), gasDataDto.MeterReadDate));
                    _logger.LogError("UpdateGasDataRecordAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                var existingRecord = await _db.GasMeterReadings.FirstOrDefaultAsync(g => g.MeterReadDate == gasDataDto.MeterReadDate);
                if (existingRecord == null)
                {
                    var result = Result<bool>.Failure(
                        ErrorCode.NotFound, 
                        InternalErrorMessages.RecordNotFound(
                            nameof(GasMeterReading), 
                            gasDataDto.MeterReadDate.ToString()
                        ));
                    _logger.LogError("UpdateGasDataRecordAsync: {Result}", result.ErrorMessage);
                    return result;
                }

                // Update only the fields that have changed
                existingRecord.Consumption = gasDataDto.Consumption;
                existingRecord.AvgTemperature = gasDataDto.AvgTemperature;

                await _db.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateGasDataRecordAsync: Exception occurred while updating gas data record for {gasDataDto.MeterReadDate}: {ex.Message}");
                return Result<bool>.Failure(ErrorCode.InternalServerError, $"An error occurred while updating the gas data record: {ex.Message}");
            }
        }
        public async Task<Result<bool>> DeleteGasDataRecordAsync(DateTime date)
        {
            if (date == default)
            {
                var result = Result<bool>.Failure(
                    ErrorCode.NullOrEmpty,
                    InternalErrorMessages.NullOrEmptyParameter(nameof(date)));
                _logger.LogError("DeleteGasDataRecordAsync: {Result}", result.ErrorMessage);
                return result;
            }

            if (date > DateTime.Now)
            {
                var result = Result<bool>.Failure(
                    ErrorCode.OutOfRange,
                    InternalErrorMessages.InvalidValue(nameof(date), date));
                _logger.LogError("DeleteGasDataRecordAsync: {Result}", result.ErrorMessage);
                return result;
            }

            var existingRecord = await _db.GasMeterReadings.FirstOrDefaultAsync(g => g.MeterReadDate == date);
            if (existingRecord == null)
            {
                var result = Result<bool>.Failure(
                    ErrorCode.NotFound,
                    InternalErrorMessages.RecordNotFound(
                        nameof(GasMeterReading),
                        date.ToString()
                    ));
                _logger.LogError("DeleteGasDataRecordAsync: {Result}", result.ErrorMessage);
                return result;
            }

            _db.GasMeterReadings.Remove(existingRecord);
            await _db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        private void LogValidationFailure(string method, string errorMessage)
        {
            _logger.LogWarning("{method}: Validation failed: {errorMessage}", method, errorMessage);
        }
    }
}
