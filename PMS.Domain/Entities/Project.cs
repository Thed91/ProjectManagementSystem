using PMS.Domain.Common;
using PMS.Domain.Enums;
using PMS.Domain.Exceptions;

namespace PMS.Domain.Entities
{
    public class Project : AuditableEntity
    {
        private Project() { }

        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Key { get; private set; }
        public ProjectStatus Status { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public static Project Create(string name, string description, string key, Guid createdBy)
        {
            var project = new Project()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                Key = key.ToUpper(),
                Status = ProjectStatus.Planning,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            return project;
        }
        public void UpdateDetails(string name, string description, Guid modifiedBy)
        {
            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void Start(Guid modifiedBy)
        {
            if (Status != ProjectStatus.Planning)
            {
                throw new DomainException("The project cannot be launched, Tatus Planning");
            }

            Status = ProjectStatus.Active;
            StartDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }

        public void Complete(Guid modifiedBy)
        {
            Status = ProjectStatus.Completed;
            EndDate = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            LastModifiedBy = modifiedBy;
        }
    }
}
