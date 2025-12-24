using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Projects.Commands.CreateProject;
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

        [HttpGet("{id}")]
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
    }
}