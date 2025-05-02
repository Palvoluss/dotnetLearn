using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Controllers;
using api.Data;
using api.Dtos.Stock;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace api.Tests.Controllers
{
    public class StockControllerTests
    {
        private ApplicationDBContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new ApplicationDBContext(options);
        }

        // Test for GetAll method
        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfStocks()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);

            context.Stocks.AddRange(
                new Stock
                {
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Purchase = 150.00m,
                    LastDiv = 0.88m,
                    Industry = "Technology",
                    MarketCap = 2400000000000
                },
                new Stock
                {
                    Id = 2,
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Purchase = 250.00m,
                    LastDiv = 2.48m,
                    Industry = "Technology",
                    MarketCap = 2100000000000
                }
            );
            context.SaveChanges();

            var controller = new StockController(context);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStocks = Assert.IsAssignableFrom<IEnumerable<StockDto>>(okResult.Value);
            Assert.Equal(2, returnedStocks.Count());
        }

        // Test for GetById method - Success case
        [Fact]
        public async Task GetById_WithExistingId_ReturnsOkResult()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var stockId = 1;
            
            context.Stocks.Add(new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.88m,
                Industry = "Technology",
                MarketCap = 2400000000000
            });
            context.SaveChanges();

            var controller = new StockController(context);

            // Act
            var result = await controller.GetById(stockId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStock = Assert.IsType<StockDto>(okResult.Value);
            Assert.Equal(stockId, returnedStock.Id);
            Assert.Equal("AAPL", returnedStock.Symbol);
        }

        // Test for GetById method - Not found case
        [Fact]
        public async Task GetById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var nonExistingId = 999;

            var controller = new StockController(context);

            // Act
            var result = await controller.GetById(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test for Create method
        [Fact]
        public async Task Create_ValidStock_ReturnsCreatedAtAction()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            
            var stockDto = new CreateStockRequestDto
            {
                Symbol = "GOOG",
                CompanyName = "Alphabet Inc.",
                Purchase = 2000.00m,
                LastDiv = 0.00m,
                Industry = "Technology",
                MarketCap = 1500000000000
            };

            var controller = new StockController(context);

            // Act
            var result = await controller.Create(stockDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(StockController.GetById), createdAtActionResult.ActionName);
            
            var returnedStock = Assert.IsType<StockDto>(createdAtActionResult.Value);
            Assert.Equal(stockDto.Symbol, returnedStock.Symbol);
            
            // Verify that the stock was added to the database
            Assert.Equal(1, context.Stocks.Count());
            var stockInDb = await context.Stocks.FirstOrDefaultAsync();
            Assert.Equal(stockDto.Symbol, stockInDb.Symbol);
        }

        // Test for Update method - Success case
        [Fact]
        public async Task Update_ExistingStock_ReturnsOkResult()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var stockId = 1;
            
            context.Stocks.Add(new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.88m,
                Industry = "Technology",
                MarketCap = 2400000000000
            });
            context.SaveChanges();

            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Incorporated",  // Changed
                Purchase = 160.00m,  // Changed
                LastDiv = 0.90m,  // Changed
                Industry = "Technology",
                MarketCap = 2500000000000  // Changed
            };

            var controller = new StockController(context);

            // Act
            var result = await controller.Update(stockId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedStock = Assert.IsType<StockDto>(okResult.Value);
            
            Assert.Equal(updateDto.CompanyName, returnedStock.CompanyName);
            Assert.Equal(updateDto.Purchase, returnedStock.Purchase);
            Assert.Equal(updateDto.LastDiv, returnedStock.LastDiv);
            Assert.Equal(updateDto.MarketCap, returnedStock.MarketCap);
            
            // Verify that the stock was updated in the database
            var stockInDb = await context.Stocks.FindAsync(stockId);
            Assert.Equal(updateDto.CompanyName, stockInDb.CompanyName);
        }

        // Test for Update method - Not found case
        [Fact]
        public async Task Update_NonExistingStock_ReturnsNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var nonExistingId = 999;
            
            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "TEST",
                CompanyName = "Test Company",
                Purchase = 100.00m,
                LastDiv = 0.50m,
                Industry = "Testing",
                MarketCap = 1000000000
            };

            var controller = new StockController(context);

            // Act
            var result = await controller.Update(nonExistingId, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        // Test for Delete method - Success case
        [Fact]
        public async Task Delete_ExistingStock_ReturnsNoContent()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var stockId = 1;
            
            context.Stocks.Add(new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.00m,
                LastDiv = 0.88m,
                Industry = "Technology",
                MarketCap = 2400000000000
            });
            context.SaveChanges();

            var controller = new StockController(context);

            // Act
            var result = await controller.Delete(stockId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            
            // Verify that the stock was removed from the database
            Assert.Equal(0, context.Stocks.Count());
        }

        // Test for Delete method - Not found case
        [Fact]
        public async Task Delete_NonExistingStock_ReturnsNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            var context = GetDbContext(dbName);
            var nonExistingId = 999;

            var controller = new StockController(context);

            // Act
            var result = await controller.Delete(nonExistingId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}