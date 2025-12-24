using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.ProjectTasks.Commands.CreateProjectTask;

public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;

    public CreateProjectTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTaskDto> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = ProjectTask.Create(
            request.ProjectId,
            request.Title,
            request.Description,
            request.TaskKey,
            request.ReporterId,
            request.CreatedBy
        );
        _context.ProjectTasks.Add(projectTask);
        await _context.SaveChangesAsync(cancellationToken);
        return new ProjectTaskDto
        {
            Id = projectTask.Id,
            ProjectId = projectTask.ProjectId,
            Title = projectTask.Title,
            Description = projectTask.Description,
            TaskKey = projectTask.TaskKey,
            Status = projectTask.Status.ToString(),
            Priority = projectTask.Priority.ToString(),
            Type = projectTask.Type.ToString(),
            AssigneeId = projectTask.AssigneeId,
            ReporterId = projectTask.ReporterId,
            DueDate = projectTask.DueDate,
            EstimatedHours = projectTask.EstimatedHours,
            ActualHours = projectTask.ActualHours,
            ParentTaskId = projectTask.ParentTaskId,
            CreatedBy = projectTask.CreatedBy,
            LastModifiedBy = projectTask.LastModifiedBy,
            CreatedAt = projectTask.CreatedAt,
            UpdatedAt = projectTask.UpdatedAt,
            IsDeleted = projectTask.IsDeleted
        };
    }
}