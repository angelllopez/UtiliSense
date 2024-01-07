using Microsoft.AspNetCore.Mvc;
using Moq;
using UtiliSense.api.Controllers.Gas;
using UtiliSense.data.Models;
using UtiliSense.service.Contracts;

namespace UtiliSense.test.UtiliSense.api.test.Controllers.Gas
{
    public class GasDataControllerTests
    {
        [Fact]
        public async Task GetGasDataAsync_ReturnsOk_WhenRecordsExist()
        {
            // Arrange
            var stubList = new List<GasData> { new GasData() };
            var mockService = new Mock<IGasDataService>(MockBehavior.Strict);

            mockService.Setup(s => s.GetAllGasDataAsync()).ReturnsAsync(stubList);

            var controller = new GasDataController(mockService.Object);

            // Act
            var result = await controller.GetGasDataAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
