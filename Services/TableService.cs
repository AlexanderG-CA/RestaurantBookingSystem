using RestaurantBookingSystem.Data;
using RestaurantBookingSystem.DTOs.Requests;
using RestaurantBookingSystem.DTOs.Response;
using RestaurantBookingSystem.Interfaces;
using RestaurantBookingSystem.Repositories;

namespace RestaurantBookingSystem.Services
{
    public class TableService : ITableService
    {
        private readonly IRepository<Table> _tableRepository;

        public TableService(IRepository<Table> tableRepository)
        {
            _tableRepository = tableRepository;
        }

        public async Task<IEnumerable<TableResponse>> GetAllTablesAsync()
        {
            var tables = await _tableRepository.GetAllAsync();
            return tables.Select(t => new TableResponse
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity
            });
        }

        public async Task<TableResponse> GetTableByIdAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null) return null;

            return new TableResponse
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };
        }

        public async Task<TableResponse> CreateTableAsync(CreateTableRequest request)
        {
            var table = new Table
            {
                TableNumber = request.TableNumber,
                Capacity = request.Capacity
            };

            await _tableRepository.AddAsync(table);
            await _tableRepository.SaveChangesAsync();

            return new TableResponse
            {
                Id = table.Id,
                TableNumber = table.TableNumber,
                Capacity = table.Capacity
            };
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);
            if (table == null) return false;

            _tableRepository.Delete(table);
            return await _tableRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AvailableTableResponse>> GetAvailableTablesAsync(DateTime date, TimeSpan time, int guests)
        {
            var availableTables = await ((ITableRepository)_tableRepository).GetAvailableTablesAsync(date, time, guests);
            return availableTables.Select(t => new AvailableTableResponse
            {
                Id = t.Id,
                TableNumber = t.TableNumber,
                Capacity = t.Capacity
            });
        }
    }
}
