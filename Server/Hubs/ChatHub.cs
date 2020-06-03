using Microsoft.AspNetCore.SignalR;
using Server.Helpers;
using System;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        public async override Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public Task SendMessageToAll(string user, string message)
            => Clients.All.SendAsync("ReceiveMessage", user, message);

        public Task SendMessageToCaller(string user, string message)
           => Clients.Caller.SendAsync("ReceiveMessage", user, message);

        public Task SendMessageToUser(string connectionId, string user, string message)
        {
            Clients.Caller.SendAsync("ReceiveMessage", user, message);
            return Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }
    }
}
