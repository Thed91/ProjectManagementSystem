using MediatR;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommand : IRequest<ProjectTaskDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid ModifiedBy { get; set; }
}
