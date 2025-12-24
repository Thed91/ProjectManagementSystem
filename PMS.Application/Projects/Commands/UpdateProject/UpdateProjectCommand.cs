using MediatR;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
