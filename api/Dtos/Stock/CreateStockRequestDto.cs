using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto

    {
        [Required]
        [MaxLength(10, ErrorMessage = "Symbol must be less than 10 characters")]
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        [Range(1, 1000000000000000000, ErrorMessage = "Purchase must be between 1 and 1 Billion")]
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }
    }
}
