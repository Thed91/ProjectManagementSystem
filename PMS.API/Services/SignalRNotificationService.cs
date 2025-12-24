using Microsoft.AspNetCore.SignalR;
using PMS.API.Hubs;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Models;

namespace PMS.API.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly ILogger<SignalRNotificationService> _logger;

    public SignalRNotificationService(
        IHubContext<NotificationHub, INotificationClient> hubContext,
        ILogger<SignalRNotificationService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task SendToUserAsync(Guid userId, NotificationDto notification)
    {
        try
        {
            notification.Id = Guid.NewGuid();
            notification.Timestamp = DateTime.UtcNow;
            notification.UserId = userId;

            await _hubContext.Clients
                .Group(GetUserGroup(userId))
                .ReceiveNotification(notification);

            _logger.LogInformation(
                "Notification sent to user {UserId}: {Type} - {Message}",
                userId, notification.Type, notification.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user {UserId}", userId);
            throw;
        }
    }

    public async Task SendToProjectAsync(Guid projectId, NotificationDto notification)
    {
        try
        {
            notification.Id = Guid.NewGuid();
            notification.Timestamp = DateTime.UtcNow;
            notification.ProjectId = projectId;

            await _hubContext.Clients
                .Group(GetProjectGroup(projectId))
                .ReceiveNotification(notification);

            _logger.LogInformation(
                "Notification sent to project {ProjectId}: {Type} - {Message}",
                projectId, notification.Type, notification.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to project {ProjectId}", projectId);
            throw;
        }
    }

    public async Task SendToAllAsync(NotificationDto notification)
    {
        try
        {
            notification.Id = Guid.NewGuid();
            notification.Timestamp = DateTime.UtcNow;

            await _hubContext.Clients.All.ReceiveNotification(notification);

            _logger.LogInformation(
                "Broadcast notification sent: {Type} - {Message}",
                notification.Type, notification.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting notification");
            throw;
        }
    }

    public async Task SendToUsersAsync(IEnumerable<Guid> userIds, NotificationDto notification)
    {
        try
        {
            notification.Id = Guid.NewGuid();
            notification.Timestamp = DateTime.UtcNow;

            var groups = userIds.Select(GetUserGroup).ToList();

            await _hubContext.Clients
                .Groups(groups)
                .ReceiveNotification(notification);

            _logger.LogInformation(
                "Notification sent to {Count} users: {Type} - {Message}",
                userIds.Count(), notification.Type, notification.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to multiple users");
            throw;
        }
    }

    #region Helper Methods

    private static string GetUserGroup(Guid userId) => $"user_{userId}";
    private static string GetProjectGroup(Guid projectId) => $"project_{projectId}";

    #endregion
}
