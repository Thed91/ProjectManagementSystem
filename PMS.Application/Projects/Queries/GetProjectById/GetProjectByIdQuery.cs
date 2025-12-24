using MediatR;
using PMS.Application.Projects.DTOs;

namespace PMS.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public Guid Id { get; set;}
    }
}
