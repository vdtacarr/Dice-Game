using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRExample.Hubs
{
    public class MyHub : Hub
    {
        //static List<string> clients = new List<string>();

        public async Task SendMessageAsync(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //public async override Task OnConnectedAsync()
        //{
        //    clients.Add(Context.ConnectionId);
        //    await Clients.All.SendAsync("clients", clients);
        //    await Clients.All.SendAsync("userJoined", $"{Context.ConnectionId}");
        //}

        //async public override Task OnDisconnectedAsync(Exception exception)
        //{
        //    clients.Remove(Context.ConnectionId);
        //    await Clients.All.SendAsync("clients", clients);
        //    await Clients.All.SendAsync("userLeaved", $"{Context.ConnectionId}");
        //}
    }
}
