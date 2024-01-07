using UtiliSense.data.Models;

namespace UtiliSense.service.Contracts
{
    public interface ISolarDataService
    {
        Task<IEnumerable<SolarData>> GetAllSolarDataAsync();
        Task<SolarData> GetSolarDataByDay(DateTime date);
        Task<IEnumerable<SolarData>> GetSolarDataByMonth(DateTime date);
        Task<IEnumerable<SolarData>> GetSolarDataByYear(DateTime date);
        Task<bool> CreateSolarDataRecord(SolarData solarData);
        Task<bool> UpdateSolarDataRecord(SolarData solarData);
        Task<bool> DeleteSolarDataRecord(int id);
    }
}
