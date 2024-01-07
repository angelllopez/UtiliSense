using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.service
{
    public class WaterDataService : IWaterDataService
    {
        public Task<bool> CreateWaterDataRecord(WaterData waterData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteWaterDataRecord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WaterData>> GetAllWaterDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WaterData> GetWaterDataByDay(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WaterData>> GetWaterDataByMonth(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WaterData>> GetWaterDataByYear(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateWaterDataRecord(WaterData waterData)
        {
            throw new NotImplementedException();
        }
    }
}
