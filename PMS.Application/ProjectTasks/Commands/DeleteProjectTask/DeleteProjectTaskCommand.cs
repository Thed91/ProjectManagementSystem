using MediatR;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommand : IRequest<ProjectTaskDto>
{
    public Guid Id { get; set; }
    public Guid ModifiedBy { get; set; }
}