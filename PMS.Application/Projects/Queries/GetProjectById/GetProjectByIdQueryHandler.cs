using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {request.Id} not found");
            }
            return _mapper.Map<ProjectDto>(project);
        }
    }
}
