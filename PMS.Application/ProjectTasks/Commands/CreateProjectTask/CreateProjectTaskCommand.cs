using MediatR;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.CreateProjectTask;

public class CreateProjectTaskCommand : IRequest<ProjectTaskDto>
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TaskKey { get; set; }
    public Guid ReporterId { get; set; }
    public Guid CreatedBy { get; set; }
}