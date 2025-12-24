using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PMS.Application.Common.Models;
using System.Security.Claims;

namespace PMS.API.Hubs;

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    private readonly ILogger<NotificationHub> _logger;

    public NotificationHub(ILogger<NotificationHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        var userName = GetUserName();

        if (userId.HasValue)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId.Value));

            _logger.LogInformation(
                "User {UserName} ({UserId}) connected with ConnectionId: {ConnectionId}",
                userName, userId, Context.ConnectionId);

            await Clients.Caller.ConnectionStatus("Connected");
        }
        else
        {
            _logger.LogWarning("User connected without valid userId. ConnectionId: {ConnectionId}",
                Context.ConnectionId);
            Context.Abort();
            return;
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        var userName = GetUserName();

        if (exception != null)
        {
            _logger.LogError(exception,
                "User {UserName} ({UserId}) disconnected with error. ConnectionId: {ConnectionId}",
                userName, userId, Context.ConnectionId);
        }
        else
        {
            _logger.LogInformation(
                "User {UserName} ({UserId}) disconnected. ConnectionId: {ConnectionId}",
                userName, userId, Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    public async Task JoinProject(Guid projectId)
    {
        var userId = GetUserId();
        var userName = GetUserName();

        if (!userId.HasValue)
        {
            _logger.LogWarning("Attempt to join project without valid userId");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, GetProjectGroup(projectId));

        _logger.LogInformation(
            "User {UserName} ({UserId}) joined project {ProjectId}",
            userName, userId, projectId);

        await Clients.OthersInGroup(GetProjectGroup(projectId))
            .UserJoined(userName ?? "Unknown", userId.Value);
    }

    public async Task LeaveProject(Guid projectId)
    {
        var userId = GetUserId();
        var userName = GetUserName();

        if (!userId.HasValue)
        {
            _logger.LogWarning("Attempt to leave project without valid userId");
            return;
        }

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetProjectGroup(projectId));

        _logger.LogInformation(
            "User {UserName} ({UserId}) left project {ProjectId}",
            userName, userId, projectId);

        await Clients.OthersInGroup(GetProjectGroup(projectId))
            .UserLeft(userName ?? "Unknown", userId.Value);
    }

    public async Task SendTestNotification()
    {
        var userId = GetUserId();

        if (!userId.HasValue)
            return;

        var notification = new NotificationDto
        {
            Id = Guid.NewGuid(),
            Type = "Test",
            Title = "Test Notification",
            Message = "This is a test notification from SignalR",
            Timestamp = DateTime.UtcNow,
            UserId = userId
        };

        await Clients.Caller.ReceiveNotification(notification);
    }
    
    private Guid? GetUserId()
    {
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? Context.User?.FindFirst("sub")?.Value
                       ?? Context.User?.FindFirst("userId")?.Value;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private string? GetUserName()
    {
        return Context.User?.FindFirst(ClaimTypes.Name)?.Value
            ?? Context.User?.FindFirst("name")?.Value
            ?? Context.User?.Identity?.Name;
    }

    private static string GetUserGroup(Guid userId) => $"user_{userId}";

    private static string GetProjectGroup(Guid projectId) => $"project_{projectId}";
}
