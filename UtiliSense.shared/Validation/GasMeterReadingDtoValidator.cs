using FluentValidation;
using UtiliSense.shared.DTOs;

namespace UtiliSense.shared.Validation;

/// <summary>
/// Provides validation rules for the GasMeterReadingDto to ensure that meter reading data is complete and within
/// acceptable ranges.
/// </summary>
/// <remarks>This validator enforces that the meter read date is specified, consumption is non-negative, and the
/// average temperature falls within historically observed extremes in Fahrenheit. Use this class to validate incoming
/// gas meter reading data before processing or storage.</remarks>
public class GasMeterReadingDtoValidator : AbstractValidator<GasMeterReadingDto>
{
    public GasMeterReadingDtoValidator()
    {
        RuleFor(x => x.MeterReadDate).NotEmpty().WithMessage("MeterReadDate is required.");
        RuleFor(x => x.Consumption).InclusiveBetween(0, double.MaxValue)
            .WithMessage("Consumption must be a positive number.");
        // Temperature range based on historical extremes on Earth in Fahrenheit.
        RuleFor(x => x.AvgTemperature).InclusiveBetween(-134, 129)
            .WithMessage("AvgTemperature must be between −134 °F and 129 °F.");
    }
}
