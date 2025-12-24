using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.Application.Projects.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, PaginatedList<ProjectDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetProjectsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Projects.CountAsync(cancellationToken);
            var projects = await _context.Projects
                 .Skip((request.PageNumber - 1) * request.PageSize)
                 .Take(request.PageSize)
                 .Select(project => new ProjectDto
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
                 })
                 .ToListAsync(cancellationToken);
            return new PaginatedList<ProjectDto>(projects, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
