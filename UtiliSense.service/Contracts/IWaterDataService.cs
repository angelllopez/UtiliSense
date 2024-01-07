using UtiliSense.data.Models;

namespace UtiliSense.service.Contracts
{
    public interface IWaterDataService
    {
        Task<IEnumerable<WaterData>> GetAllWaterDataAsync();
        Task<WaterData> GetWaterDataByDay(DateTime date);
        Task<IEnumerable<WaterData>> GetWaterDataByMonth(DateTime date);
        Task<IEnumerable<WaterData>> GetWaterDataByYear(DateTime date);
        Task<bool> CreateWaterDataRecord(WaterData waterData);
        Task<bool> UpdateWaterDataRecord(WaterData waterData);
        Task<bool> DeleteWaterDataRecord(int id);
    }
}
