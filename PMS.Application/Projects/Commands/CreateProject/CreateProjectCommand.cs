using MediatR;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Key { get; set; }
        public Guid CreatedBy { get; set; }
    }
}