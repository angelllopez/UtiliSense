using UtiliSense.api.Core.shared;
using UtiliSense.shared.DTOs;

namespace UtiliSense.service.Contracts
{
    public interface IGasDataService
    {
        Task<Result<IEnumerable<GasMeterReadingDto>>> GetAllGasDataAsync();
        Task<Result<GasMeterReadingDto>> GetGasDataByDayAsync(DateTime date);
        Task<Result<IEnumerable<GasMeterReadingDto>>> GetGasDataByMonthAsync(DateTime date);
        Task<Result<IEnumerable<GasMeterReadingDto>>> GetGasDataByYearAsync(DateTime date);
        Task<Result<GasMeterReadingDto>> CreateGasDataRecordAsync(GasMeterReadingDto gasData);
        Task<Result<bool>> UpdateGasDataRecordAsync(GasMeterReadingDto gasData);
        Task<Result<bool>> DeleteGasDataRecordAsync(DateTime date);
    }
}
