using Microsoft.AspNetCore.Mvc;
using Moq;
using UtiliSense.api.Controllers.Gas;
using UtiliSense.api.Core.shared;
using UtiliSense.service.Contracts;
using UtiliSense.shared.DTOs;

namespace UtiliSense.test.UtiliSense.api.test.Controllers.Gas;

public class GasDataControllerTests
{
    [Fact]
    public async Task GetGasDataAsync_ReturnsOk_WhenRecordsExist()
    {
        // Arrange
        var stubList = new List<GasMeterReadingDto> { new() };
        var stubLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger<GasDataController>.Instance;

        var mockService = new Mock<IGasDataService>(MockBehavior.Strict);

        mockService.Setup(s => s.GetAllGasDataAsync())
            .ReturnsAsync(Result<IEnumerable<GasMeterReadingDto>>.Success(stubList));

        var controller = new GasDataController(mockService.Object, stubLogger);

        // Act
        var response = await controller.GetAllGasDataAsync();

        // Assert
        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response.Result);
    }
}


