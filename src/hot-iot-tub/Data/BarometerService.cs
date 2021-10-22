using Iot.Device.Bmp180;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Threading.Tasks;

namespace hot_iot_tub.Data
{
    public class BarometerService
    {
        public Task<Barometer> GetForecastAsync()
        {
            var i2cSettings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            try
            {
                using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
                using var bmp180 = new Bmp180(i2cDevice);

                var tempValue = bmp180.ReadTemperature();
                var preValue = bmp180.ReadPressure();
                var altValue = bmp180.ReadAltitude();

                return Task.FromResult(new Barometer
                {
                    TemperatureC = tempValue.DegreesCelsius,
                    TemperatureF = tempValue.DegreesFahrenheit,
                    PressureHectopascals = preValue.Hectopascals,
                    Altitude = altValue.Meters
                });
            }
            catch (Exception exception)
            {

                throw;
            }
        }
    }
}
