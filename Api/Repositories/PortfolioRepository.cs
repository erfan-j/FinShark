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
            var portfolios = await _context.portfolios
           .Where(p => p.UserId == id)
           .ToListAsync();

            return _mapper.Map<List<Stock>>(portfolios);
        }
    }
}
