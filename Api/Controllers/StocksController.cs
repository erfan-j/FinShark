using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Api.Dtos.Stocks;
using AutoMapper;
using Api.Interfaces;
using Api.Dtos;

namespace Api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StocksController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStockRepository _stockRepository;

        public StocksController(IMapper mapper, IStockRepository stockRepository)
        {
            _mapper = mapper;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject input)
        {
            var stocks = await _stockRepository.GetListAsync(input);
            var stockDto = _mapper.Map<List<Stock>, List<StockDto>>(stocks.Result);
            var pagedResult = new PagedResultDto<StockDto>(input.Page, stocks.TotalCount, input.PageSize, stockDto);
            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockRepository.FindAsync(id);

            if (stock == null) { return NotFound(); }

            var stockDto = _mapper.Map<Stock>(stock);
            return Ok(stockDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Create(CreateStockDto input)
        {
            var stock = _mapper.Map<CreateStockDto, Stock>(input);
            await _stockRepository.CreateAsync(stock);
            var stockDto = _mapper.Map<Stock>(stock);
            return CreatedAtAction(nameof(GetById), new { id = stock.Id }, stockDto);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto input)
        {
            var stock = _mapper.Map<UpdateStockDto, Stock>(input);
            var updatedStok =  await _stockRepository.UpdateAsync(id, stock);
            if (updatedStok is null) { return NotFound(); }
            var stockDto = _mapper.Map<Stock>(stock); //TODO: there should be another way instead of two mappings.
            return Ok(stockDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedStock = await _stockRepository.DeleteAsync(id);
            if (deletedStock is null) { return NotFound(); }
            return Ok();
        }

    }
}
