using DataModels;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_iot_tub.SignalRHubs
{
    public class BarometerHub : Hub
    {
        public Task SendBarometerValues(Barometer barometer)
        {
            return Clients.All.SendAsync("ReceiveMessage", barometer);
        }
    }
}
