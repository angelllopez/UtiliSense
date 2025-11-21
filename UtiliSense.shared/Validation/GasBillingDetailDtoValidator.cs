using FluentValidation;
using UtiliSense.shared.DTOs;

namespace UtiliSense.shared.Validation;

public class GasBillingDetailDtoValidator : AbstractValidator<GasBillingDetailDto>
{
    public GasBillingDetailDtoValidator()
    {
        RuleFor(x => x.From).NotEmpty().WithMessage("Start date is required.");
        RuleFor(x => x.To).NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.From).WithMessage("End date must be greater than or equal to start date.");
        RuleFor(x => x.TotalConsumption).GreaterThanOrEqualTo(0)
            .WithMessage("Total consumption must be a non-negative number.");
        RuleFor(x => x.TotalCost).GreaterThanOrEqualTo(0)
            .WithMessage("Total cost must be a non-negative number.");
    }
}
