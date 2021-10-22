using DataModels;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iot_worker.SignalR
{
    public interface ISignalRConnector
    {
        bool HubConnectionEstalbished { get; }

        Task<bool> ConnectWithRetryAsync(CancellationToken token);

        Task SendAsync(Barometer barometer);
    }
}
