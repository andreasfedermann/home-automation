using System;

namespace DataModels
{
    public interface IBarometer
    {
        DateTime Date { get; set; }
        double Altitude { get; set; }
        double PressureHectopascals { get; set; }
        double TemperatureC { get; set; }
        double TemperatureF { get; set; }
    }
}