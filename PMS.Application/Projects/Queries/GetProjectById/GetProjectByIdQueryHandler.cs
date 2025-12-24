using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IApplicationDbContext _context;

        public GetProjectByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {request.Id} not found");
            }
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
