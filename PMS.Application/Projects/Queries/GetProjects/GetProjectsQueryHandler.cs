using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper _mapper;

        public GetProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var totalCount = await _context.Projects.CountAsync(cancellationToken);
            var projects = await _context.Projects
                 .Skip((request.PageNumber - 1) * request.PageSize)
                 .Take(request.PageSize)
                 .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return new PaginatedList<ProjectDto>(projects, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
