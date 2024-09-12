using Api.Models;

namespace Api.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<List<Stock>> GetUserPortfoliosAsync(string id);
        Task<Portfolio> CreateAsync(Portfolio portfolio);
        Task<Portfolio?> DeleteAsync(int stockId, string userId);
    }
}
