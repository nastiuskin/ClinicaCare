using ClinicaCare.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ClinicaCare.Hubs
{
    public interface INotificationService
    {
        Task NotifyDoctorAsync(string doctorId,string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyDoctorAsync(string doctorId, string message)
        {
            var connectionId = NotificationHub._connections.GetValueOrDefault(doctorId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
            }
        }
    }
}
