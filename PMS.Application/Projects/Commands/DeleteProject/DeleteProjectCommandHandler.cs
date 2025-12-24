using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public DeleteProjectCommandHandler(IApplicationDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ProjectDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (project == null)
            {
                throw new KeyNotFoundException($"Project with id {request.Id} not found");
            }
            
            project.Delete(request.ModifiedBy);
            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ProjectDto>(project);
        }
    }
}
