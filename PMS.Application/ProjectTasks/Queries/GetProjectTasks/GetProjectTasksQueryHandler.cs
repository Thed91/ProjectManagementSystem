using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTasks;

public class GetProjectTasksQueryHandler : IRequestHandler<GetProjectTasksQuery, PaginatedList<ProjectTaskDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectTasksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ProjectTaskDto>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ProjectTasks.AsQueryable();

        if (request.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == request.ProjectId.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var projectTasks = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<ProjectTaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PaginatedList<ProjectTaskDto>(projectTasks, totalCount, request.PageNumber, request.PageSize);
    }
}
