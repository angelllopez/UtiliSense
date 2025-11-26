using UtiliSense.api.Core.shared;

namespace UtiliSense.test.Core.UtiliSense.shared;

public class ResultsTests
{
    [Fact]
    public void Success_ShouldHaveNullErrorCode()
    {
        // Arrange
        var result = Result<string>.Success("ok");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.ErrorCode);
        Assert.Equal("ok", result.Data);
    }

    [Fact]
    public void Failure_ShouldExposeErrorCodeAndMessage()
    {
        var result = Result<string>.Failure(ErrorCode.NotFound, "not found");

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorCode.NotFound, result.ErrorCode);
        Assert.Equal("not found", result.ErrorMessage);
        Assert.Null(result.Data);
    }
}
