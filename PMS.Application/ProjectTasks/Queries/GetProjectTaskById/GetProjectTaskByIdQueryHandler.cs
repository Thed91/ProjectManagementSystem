using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Queries.GetProjectTaskById;

public class GetProjectTaskByIdQueryHandler : IRequestHandler<GetProjectTaskByIdQuery, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectTaskByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectTaskDto> Handle(GetProjectTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (projectTask == null)
        {
            throw new KeyNotFoundException($"ProjectTask with id {request.Id} not found");
        }

        return _mapper.Map<ProjectTaskDto>(projectTask);
    }
}
