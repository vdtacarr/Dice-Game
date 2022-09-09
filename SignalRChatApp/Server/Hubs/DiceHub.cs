using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BlazorWebAssemblySignalRApp.Server.Hubs
{
    

    public class DiceHub : Hub
    {
        static ConcurrentDictionary<string, string> clientList = new ConcurrentDictionary<string, string>();
        static List<string> _messages = new List<string>();
        public override async Task OnConnectedAsync()
        {;
            await Clients.Caller.SendAsync("GetConnectionId", this.Context.ConnectionId);
            await Groups.AddToGroupAsync(this.Context.ConnectionId, "A");

        }
        public async Task<bool> AddList(string userName, string connectionId)
        {
            var result = clientList.TryAdd(userName, connectionId);

            if (result && clientList.Count == 2)
            {
                await GetPlayers(userName);
            }
            return result;
        }

        public async Task Broadcast(string username, string message)
        {

            await Clients.All.SendAsync("Broadcast", username, message);
        }

        public async Task GetPlayers(string userName)
        {
            //    if (clientList.Any(cl => cl.Key != userName))
            //    {
            //var player2 = clientList.First(cl => cl.Key != userName);
            //var player1 = clientList.First(cl => cl.Key == userName);

            
                var other = clientList.FirstOrDefault(x => x.Value != this.Context.ConnectionId);
                await Clients.GroupExcept("A", other.Value).SendAsync("FetchUser", other.Key, other.Value);
                await Clients.GroupExcept("A", this.Context.ConnectionId).SendAsync("FetchUser", userName, this.Context.ConnectionId);
            //await Clients.Client(this.Context.ConnectionId).SendAsync("FetchUser", asd.Key, asd.Value);
            //await Clients.Client(player2.Value).SendAsync("FetchUser", player1.Key, player1.Value);

            clientList.TryRemove(other.Key, out _);
            clientList.TryRemove(this.Context.ConnectionId, out _);
            //}
        }

        public async Task SenDice(string connectionID, string userName, int diceOne, int diceTwo)
        {
            await Clients.Client(connectionID).SendAsync("GetDice", userName, diceOne, diceTwo);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            string userName = clientList.Where(entry => entry.Value == connectionId)
              .Select(entry => entry.Key).FirstOrDefault();
            clientList.TryRemove(userName, out _);
            await base.OnDisconnectedAsync(exception);
        }
    }
}