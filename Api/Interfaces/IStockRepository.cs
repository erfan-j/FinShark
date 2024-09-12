using Api.Dtos.Stocks;
using Api.Models;
using Api.Utils;

namespace Api.Interfaces
{
    public interface IStockRepository
    {
        Task<CountedResult<Stock>> GetListAsync(QueryObject input);
        Task<Stock?> FindAsync(int id);
        Task<Stock?> FindBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, Stock stock);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> CheckExistAsync(int? id);
    }
}
