using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ClinicaCare.SignalR
{
    public class NotificationHub : Hub
    {
        public static readonly Dictionary<string, string> _connections = new();

        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var cleanUserId = userId.Replace("UserId { Value = ", "").Replace(" }", "").Trim();
                _connections[cleanUserId] = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }


        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                var cleanUserId = userId.Replace("UserId { Value = ", "").Replace(" }", "").Trim();
                _connections.Remove(cleanUserId);
            }

            return base.OnDisconnectedAsync(exception);
        }


    //    public Task SendNotificationToDoctor(string doctorId, string message)
    //    {
    //        if (_connections.TryGetValue(doctorId, out var connectionId))
    //        {
    //            return Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
    //        }

    //        return Task.CompletedTask;
    //    }
    }
}

