using PMS.Domain.Common;
using PMS.Domain.Enums;
using PMS.Domain.Exceptions;

namespace PMS.Domain.Entities
{
    public class ProjectTask : BaseEntity
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

        private ProjectTask() { }

        public static ProjectTask Create(Guid projectId, string title, string description, string taskKey, Guid reporterId)
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
                CreatedAt = DateTime.UtcNow
            };
            return task;
        }

        public void AssignTo(Guid userId)
        {
            AssigneeId = userId;
            UpdatedAt = DateTime.UtcNow;
        }
            
        public void ChangeStatus(Enums.TaskStatus newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPriority(TaskPriority priority)
        {
            Priority = priority;
            UpdatedAt = DateTime.UtcNow;
        }
        public void SetDueDate(DateTime dueDate)
        {
            if (dueDate < DateTime.UtcNow)
            {
                throw new DomainException("The deadline has passed");
            }
            DueDate = dueDate;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
