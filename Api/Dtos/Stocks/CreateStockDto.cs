using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Stocks
{
    public class CreateStockDto
    {
        [Required, MaxLength(100)]
        public string Symbol { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;
        [Required, Range(0.001,1000000000)]
        public decimal? Purchase { get; set; }
        [Required, Range(0.001, 100)]
        public decimal? LastDiv { get; set; }
        [Required, MaxLength(150)]
        public string Industry { get; set; } = string.Empty;
        [Required]
        public long? MarketCap { get; set; }
    }
}