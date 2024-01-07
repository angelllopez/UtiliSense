using UtiliSense.data.Models;

namespace UtiliSense.service.Contracts
{
    public interface IGridDataService
    {
        Task<IEnumerable<GridData>> GetAllGridDataAsync();
        Task<GridData> GetGridDataByDay(DateTime date);
        Task<IEnumerable<GridData>> GetGridDataByMonth(DateTime date);
        Task<IEnumerable<GridData>> GetGridDataByYear(DateTime date);
        Task<bool> CreateGridDataRecord(GridData gridData);
        Task<bool> UpdateGridDataRecord(GridData gridData);
        Task<bool> DeleteGridDataRecord(int id);
    }
}
