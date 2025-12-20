using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        void IEntityTypeConfiguration<Project>.Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
            builder.HasIndex(x => x.Key).IsUnique();
            builder.Property(x => x.Key).HasMaxLength(10).IsRequired();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }

    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.ProjectId);
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(5000).IsRequired();
            builder.HasIndex(x => x.TaskKey).IsUnique();
            builder.Property(x => x.TaskKey).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Status).HasConversion<string>();
            builder.Property(x => x.Priority).HasConversion<string>();
            builder.Property(x => x.Type).HasConversion<string>();
            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
