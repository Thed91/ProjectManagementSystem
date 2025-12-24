using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IApplicationDbContext context,  IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {request.Id} not found");
            }

            project.UpdateDetails(
                request.Name, 
                request.Description,
                request.ModifiedBy);

            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
