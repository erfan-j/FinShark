using Api.Data;
using Api.Interfaces;
using Api.Models;
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

        public async Task<List<Stock>> GetListAsync()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return stocks;
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

        public async Task<Stock?> UpdateAsync(int id ,Stock stock)
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
    }
}