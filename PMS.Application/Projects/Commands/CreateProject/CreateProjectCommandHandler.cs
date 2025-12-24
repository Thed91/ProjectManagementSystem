using AutoMapper;
using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.Common.Models;
using PMS.Application.Projects.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public CreateProjectCommandHandler(
            IApplicationDbContext context,
            IMapper mapper,
            INotificationService notificationService)
        {
            _context = context;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = Project.Create(
                request.Name,
                request.Description,
                request.Key,
                request.CreatedBy);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<ProjectDto>(project);

            await _notificationService.SendToAllAsync(new NotificationDto
            {
                Type = nameof(NotificationType.ProjectCreated),
                Title = "New Project Created",
                Message = $"Project '{project.Name}' has been created",
                Data = result
            });

            return result;
        }
    }
}