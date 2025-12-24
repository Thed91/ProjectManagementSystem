
using MediatR;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
