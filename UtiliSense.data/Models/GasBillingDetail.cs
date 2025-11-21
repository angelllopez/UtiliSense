namespace UtiliSense.data.Models;

public class GasBillingDetail
{
    public int Id { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public double TotalConsumption { get; set; }
    public DateTime DueDate { get; set; }
    public double TotalCost { get; set; }
}
