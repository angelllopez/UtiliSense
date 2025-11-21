using FluentValidation;
using UtiliSense.shared.DTOs;

namespace UtiliSense.shared.Validation
{
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
}
