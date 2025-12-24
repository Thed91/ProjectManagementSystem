using MediatR;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTasks;

public class GetProjectTasksQuery : IRequest<PaginatedList<ProjectTaskDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? ProjectId { get; set; }
}
