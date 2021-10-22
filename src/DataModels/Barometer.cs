using System;

namespace DataModels
{
    public class Barometer : IBarometer
    {
        public double TemperatureC { get; set; }

        public double TemperatureF { get; set; }

        public double PressureHectopascals { get; set; }

        public double Altitude { get; set; }

        public DateTime Date { get; set; }
    }
}