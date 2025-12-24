using MediatR;
using PMS.Application.Projects.DTOs;
using PMS.Domain.Common.Models;

namespace PMS.Application.Projects.Queries.GetProjects
{
    public class GetProjectsQuery : IRequest<PaginatedList<ProjectDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
