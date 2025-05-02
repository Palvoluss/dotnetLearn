using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(SearchSortRequestDto searchDto, SortStockRequestDto sortDto, PageRequestDto pageDto);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, Stock stock);
        Task<bool> DeleteAsync(int id);
        Task<bool> StockExistsAsync(int id);
    }
}