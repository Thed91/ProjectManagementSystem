using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTasks;

public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, PaginatedList<ProjectTaskDto>>
{
    private readonly IApplicationDbContext _context;

    public GetProjectTasksQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ProjectTaskDto>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ProjectTasks.AsQueryable();

        if (request.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == request.ProjectId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var projectTasks = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(task => new ProjectTaskDto
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                TaskKey = task.TaskKey,
                Status = task.Status.ToString(),
                Priority = task.Priority.ToString(),
                Type = task.Type.ToString(),
                AssigneeId = task.AssigneeId,
                ReporterId = task.ReporterId,
                DueDate = task.DueDate,
                EstimatedHours = task.EstimatedHours,
                ActualHours = task.ActualHours,
                ParentTaskId = task.ParentTaskId,
                CreatedBy = task.CreatedBy,
                LastModifiedBy = task.LastModifiedBy,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                IsDeleted = task.IsDeleted
            })
            .ToListAsync(cancellationToken);

        return new PaginatedList<ProjectTaskDto>(projectTasks, totalCount, request.PageNumber, request.PageSize);
    }
}
