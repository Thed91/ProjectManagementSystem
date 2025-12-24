using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        
        public DeleteProjectCommandHandler(IApplicationDbContext context) {
            _context = context;
        }
        public async Task<ProjectDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {request.Id} not found");
            }
            
            project.Delete(request.Id);
            _context.Projects.Update(project);
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
                CreatedAt = project.CreatedAt,
                UpdatedAt = project.UpdatedAt,
                IsDeleted = project.IsDeleted,
                CreatedBy = project.CreatedBy,
                LastModifiedBy = project.LastModifiedBy
            };
        }
    }
}
