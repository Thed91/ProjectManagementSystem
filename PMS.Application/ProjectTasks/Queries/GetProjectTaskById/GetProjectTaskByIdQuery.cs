using MediatR;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTaskById;

public class GetProjectTaskByIdQuery : IRequest<ProjectTaskDto>
{
    public Guid Id { get; set; }
}
