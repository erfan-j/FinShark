using Api.Data;
using Api.Dtos.Stocks;
using Api.Interfaces;
using Api.Models;
using Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _context;

        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CountedResult<Stock>> GetListAsync(QueryObject input)
        {
            var q = _context.Stocks
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .AsQueryable();
            var totalCount = q.Count();

            if (!string.IsNullOrEmpty(input.SearchValue))
            {
                q = q.Where(s => s.Industry.Contains(input.SearchValue) || s.Symbol.Contains(input.SearchValue));  
            }

            q = q.OrderByDescending(s => s.Industry);
            q = q.Skip((input.Page - 1) * input.PageSize).Take(10);

            var stocks = await q.ToListAsync();
            var result = new CountedResult<Stock>
            {
                TotalCount = totalCount,
                Result = stocks
            };

            return result;
        }

        public async Task<Stock?> FindAsync(int id)
        {
            var stock = await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(s => s.Id == id);

            return stock;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            var newStock = await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return newStock.Entity;
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var stockModel = await _context.Stocks.FindAsync(id);
            if (stockModel is null) { return null; }

            stockModel.Industry = stock.Industry;
            stockModel.CompanyName = stock.CompanyName;
            stockModel.Purchase = stock.Purchase;
            stockModel.LastDiv = stock.LastDiv;
            stockModel.Symbol = stock.Symbol;
            stockModel.MarketCap = stock.MarketCap;

            await _context.SaveChangesAsync();
            return (stockModel);
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FindAsync(id);
            if (stockModel is null) { return null; }

            _context.Remove(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;
        }

        public async Task<bool> CheckExistAsync(int? id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> FindBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }
    }

}