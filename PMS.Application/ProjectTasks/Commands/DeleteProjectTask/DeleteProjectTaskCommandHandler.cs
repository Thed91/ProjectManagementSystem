using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    public DeleteProjectTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectTaskDto> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (projectTask == null)
        {
            throw new KeyNotFoundException($"ProjectTask with id {request.Id} not found");
        }

        projectTask.Delete(request.ModifiedBy);
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