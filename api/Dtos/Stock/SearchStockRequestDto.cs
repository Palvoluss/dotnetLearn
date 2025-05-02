using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class SearchSortRequestDto
    {
        public string? Symbol { get; set; } = null; 
        public string? CompanyName { get; set; } = null;
    }
}