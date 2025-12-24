using AutoMapper;
using MediatR;
using PMS.Application.Common.Interfaces;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Entities;

namespace PMS.Application.ProjectTasks.Commands.CreateProjectTask;

public class CreateProjectTaskCommandHandler : IRequestHandler<CreateProjectTaskCommand, ProjectTaskDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateProjectTaskCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectTaskDto> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var projectTask = ProjectTask.Create(
            request.ProjectId,
            request.Title,
            request.Description,
            request.TaskKey,
            request.ReporterId,
            request.CreatedBy
        );
        _context.ProjectTasks.Add(projectTask);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProjectTaskDto>(projectTask);
    }
}