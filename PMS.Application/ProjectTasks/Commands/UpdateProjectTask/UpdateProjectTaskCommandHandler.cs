using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.UpdateProjectTask;

public class UpdateProjectTaskCommandHandler : IRequestHandler<UpdateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProjectTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectTaskDto> Handle(UpdateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
        if (projectTask == null)
        {
            throw new KeyNotFoundException($"ProjectTask with id {request.Id} not found");
        }

        projectTask.UpdateDetails(
            request.Title,
            request.Description,
            request.ModifiedBy);

        _context.ProjectTasks.Update(projectTask);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectTaskDto>(projectTask);
    }
}
