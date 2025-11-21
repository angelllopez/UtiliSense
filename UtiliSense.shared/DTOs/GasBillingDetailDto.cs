namespace UtiliSense.shared.DTOs;

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

