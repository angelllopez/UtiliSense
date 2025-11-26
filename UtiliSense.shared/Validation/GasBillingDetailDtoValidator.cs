using FluentValidation;
using UtiliSense.shared.DTOs;

namespace UtiliSense.shared.Validation;

/// <summary>
/// Provides validation rules for the GasBillingDetailDto to ensure that billing details such as dates, consumption, and
/// cost meet required constraints.
/// </summary>
/// <remarks>This validator enforces that the start and end dates are specified and that the end date is not
/// earlier than the start date. It also ensures that total consumption and total cost are non-negative values. Use this
/// class to validate GasBillingDetailDto instances before processing or persisting billing data.</remarks>
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
