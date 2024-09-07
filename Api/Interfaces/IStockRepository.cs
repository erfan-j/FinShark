using Api.Models;

namespace Api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetListAsync();
        Task<Stock?> FindAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, Stock stock);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int id);
    }
}
