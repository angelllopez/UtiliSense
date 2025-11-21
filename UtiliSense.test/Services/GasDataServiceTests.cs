using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using UtiliSense.data;
using UtiliSense.service;
using UtiliSense.shared.DTOs;
using UtiliSense.shared.Profiles;
using FluentValidation;

namespace UtiliSense.test.Services;

public class GasDataServiceTests
{
    /// <summary>
    /// Creates a new instance of <see cref="UtiliSenseDbContext"/> configured to use an in-memory database.
    /// </summary>
    /// <param name="utiliSenseInMemoryDb">The name of the in-memory database to create or connect to.</param>
    /// <returns>A new <see cref="UtiliSenseDbContext"/> instance configured with the specified in-memory database.</returns>
    private static UtiliSenseDbContext CreateInMemoryDb(string utiliSenseInMemoryDb)
    {
        var options = new DbContextOptionsBuilder<UtiliSenseDbContext>()            
            .UseInMemoryDatabase(databaseName: utiliSenseInMemoryDb)
            .Options;
        return new UtiliSenseDbContext(options);
    }


    private static GasDataService CreateService(UtiliSenseDbContext db)
    {
        var logger = NullLogger<GasDataService>.Instance;
        var loggerFactory = NullLoggerFactory.Instance;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new GasMeterReadingProfile());
        }, loggerFactory);


        var mapper = config.CreateMapper();

        // Simple pass-through validator for tests
        var validator = new InlineValidator<GasMeterReadingDto>();

        return new GasDataService(db, logger, mapper, validator);
    }

    [Fact]
    public async Task CreateGasDataRecordAsync_Success()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);

        var dto = new GasMeterReadingDto
        {
            MeterReadDate = DateTime.Now.AddDays(-1),
            Consumption = 10,
            AvgTemperature = 50
        };

        var result = await service.CreateGasDataRecordAsync(dto);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(dto.MeterReadDate, result.Data.MeterReadDate);
    }

    [Fact]
    public async Task CreateGasDataRecordAsync_NullDto_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);

        var result = await service.CreateGasDataRecordAsync(null!);

        Assert.False(result.IsSuccess);
    }
}
