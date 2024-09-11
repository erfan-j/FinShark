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
        private readonly IMapper _mapper;

        public PortfolioController(IPortfolioRepository portfolioRepository, IMapper mapper)
        {
            _portfolioRepository = portfolioRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            
            var userStocks = await _portfolioRepository.GetUserPortfoliosAsync(userId);
            var stockDto = _mapper.Map<List<Stock>, List<StockDto>>(userStocks);

            return Ok(stockDto);
        }
    }
}
