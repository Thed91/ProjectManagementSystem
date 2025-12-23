using PMS.Domain.Common;

namespace PMS.Domain.Entities
{
    public class Comment : BaseEntity
    {
        private Comment() { }
        public Guid TaskId { get; private set; }
        public Guid UserId { get; private set; }
        public string Content { get; private set; }

        public static Comment Create(Guid taskId, Guid userId, string content)
        {
            var comment = new Comment()
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };
            return comment;
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
