namespace UtiliSense.shared.DTOs;

/// <summary>
/// Data transfer object for gas data records.
/// </summary>
public class GasMeterReadingDto
{
    public int Id { get; set; } 
    /// <summary>
    /// The date and time the meter was read.
    /// Equivalent to GasBillingDetail.To.
    /// </summary>
    public DateTime MeterReadDate { get; set; }

    /// <summary>
    /// The amount of gas consumed.
    /// </summary>
    public double Consumption { get; set; }
    /// <summary>
    /// The average temperature during the meter reading period.
    /// </summary>
    public double AvgTemperature { get; set; }
}
