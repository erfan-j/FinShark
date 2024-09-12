using Api.Data;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PortfolioRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<Stock>> GetUserPortfoliosAsync(string id)
        {
            var portfolios = await _context.portfolios.Include(p => p.Stock)
           .Where(p => p.UserId == id)
           .ToListAsync();

            return _mapper.Map<List<Stock>>(portfolios);
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
                await _context.portfolios.AddAsync(portfolio);
                await _context.SaveChangesAsync();
                return portfolio;
        }

        public async Task<Portfolio?> DeleteAsync(int stockId, string userId)
        {
            var portfolio = await _context.portfolios.FirstOrDefaultAsync(p => p.StockId == stockId && p.UserId == userId);
            if (portfolio is null) { return null; }

            _context.Remove(portfolio);
            await _context.SaveChangesAsync();

            return portfolio;
        }
    }
}
