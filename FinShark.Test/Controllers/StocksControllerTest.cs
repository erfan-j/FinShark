using Api.Controllers;
using Api.Dtos;
using Api.Dtos.Stocks;
using Api.Interfaces;
using Api.Models;
using Api.Utils;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FinShark.Test.Controllers
{
    public class StocksControllerTest
    {
        private List<Stock> _stockList;
        private List<StockDto> _stockDtoList;

        private readonly Mock<IStockRepository> _mockStockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly StocksController _stocksController;

        public StocksControllerTest()
        {
            //Setup IStockRepo mock
            _mockStockRepository = new Mock<IStockRepository>();

            //Setup IMapper mock
            _mockMapper = new Mock<IMapper>();

            _stockList = new List<Stock>
        {
            new Stock { Id = 1, CompanyName = "Stock 1" },
            new Stock { Id = 2, CompanyName = "Stock 2" },
            new Stock { Id = 3, CompanyName = "Stock 3" },
            new Stock { Id = 4, CompanyName = "Stock 4" },
            new Stock { Id = 5, CompanyName = "Stock 5" },
            new Stock { Id = 6, CompanyName = "Stock 6" },
            new Stock { Id = 7, CompanyName = "Stock 7" },
            new Stock { Id = 8, CompanyName = "Stock 8" },
            new Stock { Id = 9, CompanyName = "Stock 9" },
            new Stock { Id = 10, CompanyName = "Stock 10" }
        };

            _stockDtoList = new List<StockDto>
        {
            new StockDto { Id = 1, CompanyName = "Stock 1", Symbol = "ST1" },
            new StockDto { Id = 2, CompanyName = "Stock 2", Symbol = "ST2" },
            new StockDto { Id = 3, CompanyName = "Stock 3", Symbol = "ST3" },
            new StockDto { Id = 4, CompanyName = "Stock 4", Symbol = "ST4" },
            new StockDto { Id = 5, CompanyName = "Stock 5", Symbol = "ST5" },
            new StockDto { Id = 6, CompanyName = "Stock 6", Symbol = "ST6" },
            new StockDto { Id = 7, CompanyName = "Stock 7", Symbol = "ST7" },
            new StockDto { Id = 8, CompanyName = "Stock 8", Symbol = "ST8" },
            new StockDto { Id = 9, CompanyName = "Stock 9", Symbol = "ST9" },
            new StockDto { Id = 10, CompanyName = "Stock 10", Symbol = "ST10" }
        };

            _stocksController = new StocksController(_mockMapper.Object, _mockStockRepository.Object);
        }



        [Fact]
        public async Task GetAll_ReturnsOk_WithAllStocksAsPagesResult()
        {
            //Arrange
            var queryObject = new QueryObject();

            var totalCount = _stockList.Count;

            var pagedResult = new PagedResultDto<Stock>(
                queryObject.Page,
                totalCount,
                queryObject.PageSize,
                items: _stockList.Take(queryObject.PageSize).ToList()// Simulate paginated result
            );

            var countedResult = new CountedResult<Stock>
            {
                Result = _stockList,
                TotalCount = totalCount,
            };

            _mockStockRepository.Setup(x => x.GetListAsync(It.IsAny<QueryObject>()))
                .ReturnsAsync(countedResult);

            _mockMapper.Setup(m => m.Map<List<Stock>, List<StockDto>>(It.IsAny<List<Stock>>()))
                .Returns(_stockDtoList);

            //Act
            var result = await _stocksController.GetAll(queryObject);

            //Assert
            var okResult = result as ObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var pagedResultDto = okResult.Value as PagedResultDto<StockDto>;
            pagedResultDto.Should().NotBeNull();
            pagedResultDto!.Page.Should().Be(queryObject.Page);
            pagedResultDto.PageSize.Should().Be(queryObject.PageSize);
            pagedResultDto.TotalCount.Should().Be(totalCount);
            pagedResultDto.TotalPages.Should().Be((int)Math.Ceiling((double)totalCount / queryObject.PageSize));
            pagedResultDto.HasPreviousPage.Should().Be(queryObject.Page < 1);
            pagedResultDto.HasNextPage.Should().Be(queryObject.Page < pagedResultDto.TotalPages);
            pagedResultDto.Items.Should().BeEquivalentTo(_stockDtoList.Take(queryObject.PageSize));
        }

        [Fact]
        public async Task GetAll_ReturnsOk_WithUserDesiredStocks()
        {
            //Arrange
            var queryObject = new QueryObject()
            {
                SearchValue = "Stock 8"
            };

            var totalCount = 1;

            var requiredStock = _stockList.Where(s => s.Symbol.Contains(queryObject.SearchValue) || s.CompanyName.Contains(queryObject.SearchValue)).ToList();
            var requiredStockDto = _stockDtoList.Where(dto => requiredStock.Any(stock => stock.Id == dto.Id)).ToList();

            var pagedResult = new PagedResultDto<Stock>(
                queryObject.Page,
                totalCount,
                queryObject.PageSize,
                items: requiredStock// Simulate paginated result
            );

            var countedResult = new CountedResult<Stock>
            {
                Result = requiredStock,
                TotalCount = totalCount,
            };

            _mockStockRepository.Setup(x => x.GetListAsync(It.IsAny<QueryObject>()))
                .ReturnsAsync(countedResult);

            _mockMapper.Setup(m => m.Map<List<Stock>, List<StockDto>>(requiredStock))
                .Returns(requiredStockDto);

            //Act
            var result = await _stocksController.GetAll(queryObject);

            //Assert
            var okResult = result as ObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var pagedResultDto = okResult.Value as PagedResultDto<StockDto>;
            pagedResultDto.Should().NotBeNull();
            pagedResultDto!.Page.Should().Be(queryObject.Page);
            pagedResultDto.PageSize.Should().Be(queryObject.PageSize);
            pagedResultDto.TotalCount.Should().Be(totalCount);
            pagedResultDto.TotalPages.Should().Be((int)Math.Ceiling((double)totalCount / queryObject.PageSize));
            pagedResultDto.HasPreviousPage.Should().Be(queryObject.Page < 1);
            pagedResultDto.HasNextPage.Should().Be(queryObject.Page < pagedResultDto.TotalPages);
            pagedResultDto.Items.Should().BeEquivalentTo(requiredStockDto);
        }
    }
}
