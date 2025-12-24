using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.ProjectTasks.Commands.CreateProjectTask;
using PMS.Application.ProjectTasks.Commands.DeleteProjectTask;
using PMS.Application.ProjectTasks.Commands.UpdateProjectTask;
using PMS.Application.ProjectTasks.Queries.GetProjectTaskById;
using PMS.Application.ProjectTasks.Queries.GetProjectTasks;
using PMS.Application.ProjectTasks.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectTasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectTasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ProjectTaskDto>> CreateProjectTask(CreateProjectTaskCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProjectTaskById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> GetProjectTaskById(Guid id)
    {
        var query = new GetProjectTaskByIdQuery { Id = id };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProjectTaskDto>>> GetProjectTasks([FromQuery] GetProjectTasksQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> UpdateProjectTask(Guid id, UpdateProjectTaskCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match command id");
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ProjectTaskDto>> DeleteProjectTask(Guid id, DeleteProjectTaskCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Route id does not match command id");
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
