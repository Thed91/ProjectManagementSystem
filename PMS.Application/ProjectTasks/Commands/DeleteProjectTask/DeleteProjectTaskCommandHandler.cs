using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;

namespace PMS.Application.ProjectTasks.Commands.DeleteProjectTask;

public class DeleteProjectTaskCommandHandler : IRequestHandler<DeleteProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public DeleteProjectTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectTaskDto> Handle(DeleteProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (projectTask == null)
        {
            throw new KeyNotFoundException($"ProjectTask with id {request.Id} not found");
        }

        projectTask.Delete(request.ModifiedBy);
        _context.ProjectTasks.Update(projectTask);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectTaskDto>(projectTask);
    }
}