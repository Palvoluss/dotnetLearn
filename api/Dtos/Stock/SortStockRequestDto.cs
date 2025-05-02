using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class SortStockRequestDto : SortRequestDto
    {
        public SortDirection? SortBySymbol { get; set; } = null;
        public SortDirection? SortByCompanyName { get; set; } = null;
    }
}