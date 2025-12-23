using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateProjectCommandHandler(IApplicationDbContext context)
        {
            _context = context;
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

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Key = project.Key,
                Status = project.Status.ToString(),
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedAt = project.CreatedAt
            };
        }
    }
}