using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.service
{
    public class SolarDataService : ISolarDataService
    {
        public Task<bool> CreateSolarDataRecord(SolarData solarData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSolarDataRecord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SolarData>> GetAllSolarDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SolarData> GetSolarDataByDay(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SolarData>> GetSolarDataByMonth(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SolarData>> GetSolarDataByYear(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSolarDataRecord(SolarData solarData)
        {
            throw new NotImplementedException();
        }
    }
}
