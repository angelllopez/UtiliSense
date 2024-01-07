using Microsoft.AspNetCore.Mvc;
using Moq;
using UtiliSense.data.Models;

namespace UtiliSense.test.UtiliSense.api.test.Controllers.Solar
{
    public class SolarDataControllerTests
    {
        [Fact]
        public async Task GetSolarDataAsync_ReturnsOk_WhenRecordsExist()
        {
            // Arrange
            var stubList = new List<SolarData> { new SolarData() };
            //var mockService = new Mock<SolarDataService>(MockBehavior.Strict);

            //mockService.Setup(s => s.GetAllSolarDataAsync()).ReturnsAsync(stubList);

            //var controller = new SolarDataController(mockService.Object);

            // Act
            //var result = await controller.GetSolarDataAsync();

            // Assert
            //Assert.NotEmpty(result);
            //Assert.IsType<OkObjectResult>(result);
        }
    }
}
