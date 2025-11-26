using Microsoft.EntityFrameworkCore;
using UtiliSense.data.Models;

namespace UtiliSense.data
{
    public class UtiliSenseDbContext : DbContext
    {
        public UtiliSenseDbContext(DbContextOptions<UtiliSenseDbContext> options)
            : base(options)
        {
        }

        // DbSet for your domain models
        public DbSet<GasMeterReading> GasMeterReadings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Ensure MeterReadDate is unique if you expect one record per date
            builder.Entity<GasMeterReading>()
                   .HasIndex(g => g.MeterReadDate)
                   .IsUnique();

            // Set appropriate precision for decimals
            builder.Entity<GasMeterReading>()
                   .Property(g => g.Consumption)
                   .HasPrecision(18, 4);

            builder.Entity<GasMeterReading>()
                   .Property(g => g.AvgTemperature)
                   .HasPrecision(18, 4);

            // Optional: If the `BillingMonth` property is redundant with MeterReadDate, consider ignoring it
            // builder.Entity<GasMeterReading>().Ignore(g => g.BillingMonth);
        }
    }
}
