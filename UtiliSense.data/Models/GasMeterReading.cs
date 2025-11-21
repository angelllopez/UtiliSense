namespace UtiliSense.data.Models
{
    public class GasMeterReading
    {
        public int Id { get; set; }
        public DateTime MeterReadDate { get; set; }
        public double Consumption { get; set; }
        public double AvgTemperature { get; set; }
    }
}
