using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using static api.Dtos.SortRequestDto;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        
        public async Task<List<Stock>> GetAllAsync(SearchSortRequestDto searchDto, SortStockRequestDto sortDto, PageRequestDto pageDto)
        {
            var query = _context.Stocks.Include(s => s.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.Symbol))
            {
                string symbol = searchDto.Symbol.Trim().ToLower();
                query = query.Where(s => s.Symbol.ToLower().Contains(symbol));
            }

            if (!string.IsNullOrWhiteSpace(searchDto.CompanyName))
            {   
                string companyName = searchDto.CompanyName.Trim().ToLower();
                query = query.Where(s => s.CompanyName.ToLower().Contains(companyName));
            }

            if (sortDto.SortBySymbol != null)
            {
                query = sortDto.SortBySymbol == SortDirection.ASC ? query.OrderBy(s => s.Symbol) : query.OrderByDescending(s => s.Symbol);
            }

            if (sortDto.SortByCompanyName != null)
            {
                query = sortDto.SortByCompanyName == SortDirection.ASC ? query.OrderBy(s => s.CompanyName) : query.OrderByDescending(s => s.CompanyName);
            }
            
            return await query.Skip((pageDto.PageNumber - 1) * pageDto.PageSize).Take(pageDto.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stock)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStock == null)
            {
                return null;
            }
            
            existingStock.Symbol = stock.Symbol;
            existingStock.CompanyName = stock.CompanyName;
            existingStock.Purchase = stock.Purchase;
            existingStock.LastDiv = stock.LastDiv;
            existingStock.Industry = stock.Industry;
            existingStock.MarketCap = stock.MarketCap;
            
            await _context.SaveChangesAsync();
            return existingStock;
         }

        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock == null)
            {
                return false;
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StockExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}