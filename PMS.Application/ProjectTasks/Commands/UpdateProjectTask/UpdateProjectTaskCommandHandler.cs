using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Models;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandHandler : IRequestHandler<UpdateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public UpdateProjectTaskCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        INotificationService notificationService)
    {
        _context = context;
        _mapper = mapper;
        _notificationService = notificationService;
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

        var result = _mapper.Map<ProjectTaskDto>(projectTask);

        // Send notification to project members
        await _notificationService.SendToProjectAsync(projectTask.ProjectId, new NotificationDto
        {
            Type = nameof(NotificationType.TaskUpdated),
            Title = "Task Updated",
            Message = $"Task '{projectTask.Title}' has been updated",
            Data = result,
            ProjectId = projectTask.ProjectId
        });

        // Notify assignee if exists
        if (projectTask.AssigneeId.HasValue)
        {
            await _notificationService.SendToUserAsync(projectTask.AssigneeId.Value, new NotificationDto
            {
                Type = nameof(NotificationType.TaskUpdated),
                Title = "Your Task Updated",
                Message = $"Task '{projectTask.Title}' assigned to you has been updated",
                Data = result,
                UserId = projectTask.AssigneeId.Value
            });
        }

        return result;
    }
}
