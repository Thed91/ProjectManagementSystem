using MediatR;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Projects.Commands.CreateProject;
using PMS.Application.Projects.DTOs;

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
    }
}