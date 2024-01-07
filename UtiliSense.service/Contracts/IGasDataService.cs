using UtiliSense.data.Models;

namespace UtiliSense.service.Contracts
{
    public interface IGasDataService
    {
        Task<IEnumerable<GasData>> GetAllGasDataAsync();
        Task<GasData> GetGasDataByDay(DateTime date);
        Task<IEnumerable<GasData>> GetGasDataByMonth(DateTime date);
        Task<IEnumerable<GasData>> GetGasDataByYear(DateTime date);
        Task<bool> CreateGasDataRecord(GasData gasData);
        Task<bool> UpdateGasDataRecord(GasData gasData);
        Task<bool> DeleteGasDataRecord(int id);
    }
}
