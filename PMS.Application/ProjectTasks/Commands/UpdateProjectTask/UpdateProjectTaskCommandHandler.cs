using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandHandler : IRequestHandler<UpdateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjectTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTaskDto> Handle(UpdateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (projectTask == null)
        {
            throw new KeyNotFoundException($"ProjectTask with id {request.Id} not found");
        }

        projectTask.UpdateDetails(
            request.Title,
            request.Description,
            request.ModifiedBy);

        _context.ProjectTasks.Update(projectTask);
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
