using DataModels;
using iot_worker.Sensors;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iot_worker.SignalR
{
    public class SignalRConnector : ISignalRConnector
    {
        private readonly IConfiguration _configuration;
        private IHubConnectionBuilder _hubConnectionBuilder;
        private HubConnection _hubConnection;
        private readonly ILogger<SignalRConnector> _logger;

        public SignalRConnector(
            IConfiguration configuration,
            IHubConnectionBuilder hubConnectionBuilder,
            ILogger<SignalRConnector> logger) =>
            (_configuration, _hubConnectionBuilder, _logger) =
            (configuration, hubConnectionBuilder, logger);

        public bool HubConnectionEstalbished { get; private set; }

        public async Task<bool> ConnectWithRetryAsync(CancellationToken token)
        {
            var barometerHubUrl = _configuration.GetSection("SignalR").GetValue<string>("BarometerHubUri");

            if (barometerHubUrl == null)
            {
                _logger.LogError("You have to provide the environment variable SIGNALR_HUB_ENDPOINT");
            }

            _logger.LogInformation("SignalRConnector started at: {time}", DateTimeOffset.Now);
            _hubConnection = _hubConnectionBuilder
                .WithUrl(barometerHubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.Closed += async (error) =>
            {
                HubConnectionEstalbished = false;
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _hubConnection.StartAsync();
            };

            _hubConnection.Reconnecting += error =>
            {
                Debug.Assert(_hubConnection.State == HubConnectionState.Reconnecting);

                // Notify users the connection was lost and the client is reconnecting.
                // Start queuing or dropping messages.

                HubConnectionEstalbished = false;
                return Task.CompletedTask;
            };

            _hubConnection.Reconnected += connectionId =>
            {
                Debug.Assert(_hubConnection.State == HubConnectionState.Connected);

                // Notify users the connection was reestablished.
                // Start dequeuing messages queued while reconnecting if any.

                HubConnectionEstalbished = true;
                return Task.CompletedTask;
            };

            _hubConnection.On<Barometer>("ReceiveMessage", (barometer) =>
            {
                Console.WriteLine(
                    $"Altitude: {barometer.Altitude}"
                );
            });

            var retryInterval = _configuration.GetSection("SignalR").GetValue<int>("RetryInterval");
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    _logger.LogInformation($"Trying to connect to SignalR Hub {barometerHubUrl}.");
                    await _hubConnection.StartAsync(token);
                    Debug.Assert(_hubConnection.State == HubConnectionState.Connected);
                    HubConnectionEstalbished = true;
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(_hubConnection.State == HubConnectionState.Disconnected);
                    _logger.LogInformation("Failed to connect, trying again in 5000 ms.");
                    await Task.Delay(5000, token);
                }
            }
        }

        public async Task SendAsync(Barometer barometer)
        {
            await _hubConnection.InvokeAsync("SendBarometerValues", barometer);
        }
    }
}
