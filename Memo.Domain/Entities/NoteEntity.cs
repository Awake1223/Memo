namespace Memo.Domain.Entities
{
    public class NoteEntity
    {
        public int Id { get; set; }
        public string ShortCode { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? UserId { get; set; }          // null = анонимная
        public DateTime? ExpiresAt { get; set; }  // null = навсегда
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsBurnAfterReading { get; set; } = false;
        public UserEntity? User { get; set; }
    }
}
