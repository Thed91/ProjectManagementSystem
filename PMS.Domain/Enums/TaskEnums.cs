namespace PMS.Domain.Enums
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Review,
        Done
    }
    public enum TaskPriority
    {
        Low,
        Medium,
        High,
        Critical
    }
    public enum TaskType
    {
        Task,
        Bug,
        Feature,
        Epic
    }
}
