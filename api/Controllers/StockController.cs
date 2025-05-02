using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepository;

        public StockController(ApplicationDBContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] SearchSortRequestDto  searchDto, [FromQuery] SortStockRequestDto sortDto, [FromQuery] PageRequestDto pageDto)
        {
            var stock = await _stockRepository.GetAllAsync(searchDto, sortDto, pageDto);
            return Ok(stock.Select(s => s.ToStockDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id); 
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
 
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepository.CreateAsync(stockModel);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = stockModel.Id }, 
                stockModel.ToStockDto()
                );
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var stockModel = await _stockRepository.GetByIdAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            await _stockRepository.UpdateAsync(id, updateDto.ToStockFromUpdate());
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete()]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _stockRepository.GetByIdAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }
            await _stockRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}