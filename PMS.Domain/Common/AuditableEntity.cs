namespace PMS.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public Guid CreatedBy { get; protected set; }
        public Guid? LastModifiedBy { get; protected set; }
    }
}
