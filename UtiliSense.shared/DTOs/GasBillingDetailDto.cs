namespace UtiliSense.shared.DTOs;

/// <summary>
/// Represents the details of a gas billing period, including consumption and cost information.
/// </summary>
/// <remarks>This data transfer object (DTO) is used to encapsulate information about a specific gas billing
/// period. It includes the start and end dates of the billing period, the total gas consumption, and the total
/// cost.</remarks>
public class GasBillingDetailDto
{
    public int Id { get; set; }
    /// <summary>
    /// The start date of the billing period.
    /// </summary>
    public DateTime From { get; set; }
    /// <summary>
    /// The end date of the billing period.
    /// </summary>
    public DateTime To { get; set; }
    /// <summary>
    /// The total gas consumption during the billing period.
    /// </summary>
    public double TotalConsumption { get; set; }
    /// <summary>
    /// The total cost of the gas consumed in CCF units during the billing period.
    /// </summary>
    public double TotalCost { get; set; }
}

