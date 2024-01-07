using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.service
{
    public class GasDataService : IGasDataService
    {
        public Task<bool> CreateGasDataRecord(GasData gasData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGasDataRecord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GasData>> GetAllGasDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GasData> GetGasDataByDay(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GasData>> GetGasDataByMonth(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GasData>> GetGasDataByYear(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateGasDataRecord(GasData gasData)
        {
            throw new NotImplementedException();
        }
    }
}
