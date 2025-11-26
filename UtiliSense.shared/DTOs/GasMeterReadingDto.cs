namespace UtiliSense.shared.DTOs;

/// <summary>
/// Represents a data transfer object (DTO) for gas meter readings,  including details such as the reading date, gas
/// consumption, and average temperature.
/// </summary>
/// <remarks>This DTO is typically used to transfer gas meter reading data between application layers  or
/// services. It provides essential information about a specific meter reading,  including the date of the reading, the
/// amount of gas consumed, and the average temperature  during the reading period.</remarks>
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
