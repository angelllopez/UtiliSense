namespace UtiliSense.data.Models
{
    public class GridData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal Usage { get; set; }
        public decimal Cost { get; set; }
    }
}
