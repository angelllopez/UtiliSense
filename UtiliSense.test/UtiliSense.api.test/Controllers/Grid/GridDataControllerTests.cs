using Microsoft.AspNetCore.Mvc;
using Moq;
using UtiliSense.api.Controllers.Gas;
using UtiliSense.api.Controllers.Grid;
using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.test.UtiliSense.api.test.Controllers.Grid
{
    public class GridDataControllerTests
    {
        [Fact]
        public async Task GetGridDataAsync_ReturnsOk_WhenRecordsExist()
        {
            // Arrange
            var stubList = new List<GridData> { new GridData() };
            var mockService = new Mock<IGridDataService>(MockBehavior.Strict);

            mockService.Setup(s => s.GetAllGridDataAsync()).ReturnsAsync(stubList);

            var controller = new GridDataController(mockService.Object);

            // Act
            var result = await controller.GetGridDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
