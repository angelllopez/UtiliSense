using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using UtiliSense.data;
using UtiliSense.service;
using UtiliSense.shared.DTOs;
using UtiliSense.shared.Profiles;

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
    public async Task Constructor_ThrowsArgumentNullException_WhenAnyDependencyIsNull()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var logger = NullLogger<GasDataService>.Instance;
        var loggerFactory = NullLoggerFactory.Instance;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new GasMeterReadingProfile());
        }, loggerFactory);

        var mapper = config.CreateMapper();
        var validator = new InlineValidator<GasMeterReadingDto>();

        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = new GasDataService(null!, logger, mapper, validator);
            await Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = new GasDataService(db, null!, mapper, validator);
            await Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = new GasDataService(db, logger, null!, validator);
            await Task.CompletedTask;
        });
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = new GasDataService(db, logger, mapper, null!);
            await Task.CompletedTask;
        });
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

    [Fact]
    public async Task CreateGasDataRecordAsync_InvalidDto_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var dto = new GasMeterReadingDto
        {
            MeterReadDate = DateTime.Now.AddDays(1), // Future date, invalid
            Consumption = -5, // Negative consumption, invalid
            AvgTemperature = 50
        };
        var result = await service.CreateGasDataRecordAsync(dto);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetAllGasDataAsync_NoRecords_ReturnsEmptyList()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var result = await service.GetAllGasDataAsync();
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetAllGasDataAsync_WithRecords_ReturnsRecords()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        db.GasMeterReadings.Add(new data.Models.GasMeterReading
        {
            MeterReadDate = DateTime.Now.AddDays(-2),
            Consumption = 15,
            AvgTemperature = 45
        });
        db.GasMeterReadings.Add(new data.Models.GasMeterReading
        {
            MeterReadDate = DateTime.Now.AddDays(-1),
            Consumption = 20,
            AvgTemperature = 40
        });
        await db.SaveChangesAsync();
        var result = await service.GetAllGasDataAsync();
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetGasDataByDayAsync_RecordExists_ReturnsRecord()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = DateTime.Now.AddDays(-1).Date;
        db.GasMeterReadings.Add(new data.Models.GasMeterReading
        {
            MeterReadDate = targetDate,
            Consumption = 25,
            AvgTemperature = 35
        });
        await db.SaveChangesAsync();
        var result = await service.GetGasDataByDayAsync(targetDate);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(targetDate, result.Data.MeterReadDate.Date);
    }

    [Fact]
    public async Task GetGasDataByDayAsync_RecordDoesNotExist_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = DateTime.Now.AddDays(-10).Date;
        var result = await service.GetGasDataByDayAsync(targetDate);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetGasDataByDayAsync_InvalidDate_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = DateTime.Now.AddDays(1).Date;
        var result = await service.GetGasDataByDayAsync(targetDate);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetGasDataByMonthAsync_RecordsExist_ReturnsRecords()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        db.GasMeterReadings.Add(new data.Models.GasMeterReading
        {
            MeterReadDate = targetDate.AddDays(5),
            Consumption = 30,
            AvgTemperature = 30
        });
        db.GasMeterReadings.Add(new data.Models.GasMeterReading
        {
            MeterReadDate = targetDate.AddDays(15),
            Consumption = 35,
            AvgTemperature = 25
        });
        await db.SaveChangesAsync();
        var result = await service.GetGasDataByMonthAsync(targetDate);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
    }

    [Fact]
    public async Task GetGasDataByMonthAsync_NoRecords_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var result = await service.GetGasDataByMonthAsync(targetDate);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task GetGasDataByMonthAsync_InvalidDate_ReturnsFailure()
    {
        var dbName = Guid.NewGuid().ToString();
        await using var db = CreateInMemoryDb(dbName);
        var service = CreateService(db);
        var targetDate = DateTime.Now.AddMonths(1);
        var result = await service.GetGasDataByMonthAsync(targetDate);
        Assert.False(result.IsSuccess);
    }
}
