using AutoMapper;
using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Models;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.ProjectTasks.Commands.CreateProjectTask;

public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;

    public CreateProjectTaskCommandHandler(
        IApplicationDbContext context,
        IMapper mapper,
        INotificationService notificationService)
    {
        _context = context;
        _mapper = mapper;
        _notificationService = notificationService;
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

        var result = _mapper.Map<ProjectTaskDto>(projectTask);

        // Send notification to project members
        await _notificationService.SendToProjectAsync(projectTask.ProjectId, new NotificationDto
        {
            Type = nameof(NotificationType.TaskCreated),
            Title = "New Task Created",
            Message = $"New task '{projectTask.Title}' has been created",
            Data = result,
            ProjectId = projectTask.ProjectId
        });

        return result;
    }
}