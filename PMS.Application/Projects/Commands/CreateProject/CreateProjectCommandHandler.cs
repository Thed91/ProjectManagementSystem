using AutoMapper;
using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            return _mapper.Map<ProjectDto>(project);
        }
    }
}