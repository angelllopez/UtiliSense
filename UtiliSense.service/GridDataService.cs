using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.service
{
    public class GridDataService : IGridDataService
    {
        public Task<bool> CreateGridDataRecord(GridData gridData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGridDataRecord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GridData>> GetAllGridDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GridData> GetGridDataByDay(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GridData>> GetGridDataByMonth(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GridData>> GetGridDataByYear(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateGridDataRecord(GridData gridData)
        {
            throw new NotImplementedException();
        }
    }
}
