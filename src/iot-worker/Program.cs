using iot_worker.Sensors;
using iot_worker.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Kubernetes on raspberry pi
// https://alexellisuk.medium.com/walk-through-install-kubernetes-to-your-raspberry-pi-in-15-minutes-84a8492dc95a

// Deploy .net core on kubernetes
// https://www.hanselman.com/blog/how-to-build-a-kubernetes-cluster-with-arm-raspberry-pi-then-run-net-core-on-openfaas

// .net core stuff
// https://devblogs.microsoft.com/premier-developer/demystifying-the-new-net-core-3-worker-service/

// Deployment automation
// https://requestmetrics.com/building/episode-9-running-aspnet-core-applications-using-systemd-and-ansible

namespace iot_worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IHubConnectionBuilder, HubConnectionBuilder>()
                        .AddTransient<ISignalRConnector, SignalRConnector>()
                        .AddHostedService<BarometerWorker>();
                });
    }
}
