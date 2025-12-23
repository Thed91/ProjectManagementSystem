using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;

namespace PMS.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Project> Projects { get; }
        DbSet<ProjectTask> ProjectTasks { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}