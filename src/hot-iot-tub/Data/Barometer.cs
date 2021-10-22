using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_iot_tub.Data
{
    public class Barometer
    {
        public double TemperatureC { get; set; }

        public double TemperatureF { get; set; }

        public double PressureHectopascals { get; set; }

        public double Altitude { get; set; }
    }
}
