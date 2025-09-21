using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;

namespace RestaurantBookingSystem.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<TableResponse>> GetAllTablesAsync();
        Task<TableResponse> GetTableByIdAsync(int id);
        Task<TableResponse> CreateTableAsync(CreateTableRequest request);
        Task<bool> DeleteTableAsync(int id);
        Task<IEnumerable<AvailableTableResponse>> GetAvailableTablesAsync(DateTime date, TimeSpan time, int guests);
    }
}
