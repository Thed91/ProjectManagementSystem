using PMS.Application.Common.Models;

namespace PMS.Application.Common.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Send notification to specific user
    /// </summary>
    Task SendToUserAsync(Guid userId, NotificationDto notification);

    /// <summary>
    /// Send notification to all users in a project
    /// </summary>
    Task SendToProjectAsync(Guid projectId, NotificationDto notification);

    /// <summary>
    /// Send notification to all connected users
    /// </summary>
    Task SendToAllAsync(NotificationDto notification);

    /// <summary>
    /// Send notification to multiple users
    /// </summary>
    Task SendToUsersAsync(IEnumerable<Guid> userIds, NotificationDto notification);
}
