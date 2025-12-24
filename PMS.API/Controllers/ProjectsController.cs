using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Projects.Commands.CreateProject;
using PMS.Application.Projects.Commands.DeleteProject;
using PMS.Application.Projects.Commands.UpdateProject;
using PMS.Application.Projects.Queries.GetProjectById;
using PMS.Application.Projects.DTOs;
using PMS.Application.Projects.Queries.GetProjects;
using PMS.Domain.Common.Models;

namespace PMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(CreateProjectCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(CreateProject), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> GetProjectById(Guid id)
        {
            var query = new GetProjectByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ProjectDto>>> GetProjects([FromQuery] GetProjectsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(Guid id, UpdateProjectCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(UpdateProject), new { id = result.Id }, result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> DeleteProject(Guid id, DeleteProjectCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(DeleteProject), new { id = result.Id }, result);
        }
        
    }
}