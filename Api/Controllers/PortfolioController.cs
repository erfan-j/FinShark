using Api.Dtos.Stocks;
using Api.Interfaces;
using Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("/api/portfolio")]
    [ApiController]
    [Authorize]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public PortfolioController(IPortfolioRepository portfolioRepository, IMapper mapper, IStockRepository stockRepository)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var userStocks = await _portfolioRepository.GetUserPortfoliosAsync(userId);
            var stockDto = _mapper.Map<List<Stock>, List<StockDto>>(userStocks);

            return Ok(stockDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create(string symbol)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var stock = await _stockRepository.FindBySymbolAsync(symbol);

            if (stock is null) { return NotFound("Stock not found"); }

            var userPortfolio = await _portfolioRepository.GetUserPortfoliosAsync(userId);
            if (userPortfolio.Any(p => p.Symbol == symbol)) { return BadRequest("Stock is already in Portfolio"); }

            var portfolio = new Portfolio
            {
                StockId = stock.Id,
                UserId = userId,
            };

            await _portfolioRepository.CreateAsync(portfolio);

            return Created();
        }

        [HttpDelete("{stockId}")]
        public async Task<IActionResult> Delete(int stockId)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var deletedPortfolio = await _portfolioRepository.DeleteAsync(stockId, userId);
            if (deletedPortfolio is null) { return NotFound(); }
            return Ok();
        }
    }
}
