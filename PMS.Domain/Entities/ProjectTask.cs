using PMS.Domain.Common;
using PMS.Domain.Enums;
using PMS.Domain.Exceptions;

namespace PMS.Domain.Entities
{
    public class ProjectTask : AuditableEntity
    {
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string TaskKey { get; private set; }
        public Enums.TaskStatus Status { get; private set; }
        public TaskPriority Priority { get; private set; }
        public TaskType Type { get; private set; }
        public Guid? AssigneeId { get; private set; }
        public Guid ReporterId { get; private set; }
        public DateTime? DueDate { get; private set; }
        public decimal? EstimatedHours { get; private set; }
        public decimal? ActualHours { get; private set; }
        public Guid? ParentTaskId { get; private set; }

        private ProjectTask() { }

        public static ProjectTask Create(Guid projectId, string title, string description, string taskKey, Guid reporterId, Guid createdBy)
        {
            var task = new ProjectTask
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Title = title,
                Description = description,
                TaskKey = taskKey,
                Status = Enums.TaskStatus.ToDo,
                Priority = TaskPriority.Medium,
                Type = TaskType.Task,
                ReporterId = reporterId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };
            return task;
        }

        public void AssignTo(Guid userId, Guid modifiedBy)
        {
            AssigneeId = userId;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void ChangeStatus(Enums.TaskStatus newStatus, Guid modifiedBy)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void SetPriority(TaskPriority priority, Guid modifiedBy)
        {
            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }
        public void SetDueDate(DateTime dueDate, Guid modifiedBy)
        {
            if (dueDate < DateTime.UtcNow)
            {
                throw new DomainException("The deadline has passed");
            }
            DueDate = dueDate;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void SetEstimate(decimal hours, Guid modifiedBy)
        {
            EstimatedHours = hours;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void LogTime(decimal hours, Guid modifiedBy)
        {
            ActualHours ??= 0;
            ActualHours += hours;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void SetParent(Guid parentId, Guid modifiedBy)
        {
            ParentTaskId = parentId;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
