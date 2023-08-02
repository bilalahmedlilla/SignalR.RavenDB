using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.RavenDB
{
    public class MySignalRHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Broadcast the received message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
