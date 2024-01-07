using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtiliSense.data.Models
{
    public class SolarData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal EnergyProduced { get; set; }
        public decimal EnergyUsed { get; set; }
        public decimal MaxACPowerProduced { get; set; }
    }
}
