using PMS.Application.Common.Models;

namespace PMS.API.Hubs;

/// <summary>
/// Strongly-typed interface for SignalR client methods
/// </summary>
public interface INotificationClient
{
    /// <summary>
    /// Receive a notification
    /// </summary>
    Task ReceiveNotification(NotificationDto notification);

    /// <summary>
    /// Notification when user joins project
    /// </summary>
    Task UserJoined(string userName, Guid userId);

    /// <summary>
    /// Notification when user leaves project
    /// </summary>
    Task UserLeft(string userName, Guid userId);

    /// <summary>
    /// Connection status update
    /// </summary>
    Task ConnectionStatus(string status);
}
