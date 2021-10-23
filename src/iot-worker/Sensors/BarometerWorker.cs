using DataModels;
using Iot.Device.Bmp180;
using iot_worker.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Device.I2c;
using System.Threading;
using System.Threading.Tasks;

namespace iot_worker.Sensors
{
    public class BarometerWorker : BackgroundService
    {
        private readonly ILogger<BarometerWorker> _logger;
        private readonly ISignalRConnector _signalRConnector;

        public BarometerWorker(ILogger<BarometerWorker> logger, ISignalRConnector signalRConnector) =>
            (_logger, _signalRConnector) = (logger, signalRConnector);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _signalRConnector.ConnectWithRetryAsync(stoppingToken);

            var i2cSettings = new I2cConnectionSettings(1, Bmp180.DefaultI2cAddress);
            using I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);
            using var bmp180 = new Bmp180(i2cDevice);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var valuesToSend = new Barometer
                {
                    Altitude = bmp180.ReadAltitude().Meters,
                    PressureHectopascals = bmp180.ReadPressure().Pascals,
                    TemperatureC = bmp180.ReadTemperature().DegreesCelsius,
                    TemperatureF = bmp180.ReadTemperature().DegreesFahrenheit
                };

                if (_signalRConnector.HubConnectionEstalbished)
                {
                    await _signalRConnector.SendAsync(valuesToSend);
                    _logger.LogInformation($"Altitude: {valuesToSend.Altitude}\n TemperatureC: {valuesToSend.TemperatureC}\n Pressure: {valuesToSend.PressureHectopascals}");
                }
                else
                {
                    // TODO: Maybe save the data into a database until the signalr connection is established
                    _logger.LogInformation($"Altitude: {valuesToSend.Altitude}\n TemperatureC: {valuesToSend.TemperatureC}\n Pressure: {valuesToSend.PressureHectopascals}");
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
