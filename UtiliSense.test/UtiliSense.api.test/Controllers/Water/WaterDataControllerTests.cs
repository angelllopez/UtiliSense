using Microsoft.AspNetCore.Mvc;
using Moq;
using UtiliSense.api.Controllers.Water;
using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.test.UtiliSense.api.test.Controllers.Water
{
    public class WaterDataControllerTests
    {
        [Fact]
        public async Task GetWaterDataAsync_ReturnsOk_WhenRecordsExist()
        {
            // Arrange
            var stubList = new List<WaterData> { new WaterData() };
            var mockService = new Mock<IWaterDataService>(MockBehavior.Strict);

            mockService.Setup(s => s.GetAllWaterDataAsync()).ReturnsAsync(stubList);

            var controller = new WaterDataController(mockService.Object);

            // Act
            var result = await controller.GetWaterDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
