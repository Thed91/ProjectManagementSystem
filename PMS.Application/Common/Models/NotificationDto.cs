namespace PMS.Application.Common.Models;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid? UserId { get; set; }
    public Guid? ProjectId { get; set; }
}

public enum NotificationType
{
    ProjectCreated,
    ProjectUpdated,
    ProjectDeleted,
    ProjectStatusChanged,
    TaskCreated,
    TaskUpdated,
    TaskDeleted,
    TaskAssigned,
    TaskStatusChanged,
    TaskCommented,
    UserJoinedProject,
    UserLeftProject
}
