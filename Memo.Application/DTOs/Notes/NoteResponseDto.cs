namespace Memo.Application.DTOs.Notes
{
    public class NoteResponseDto
    {
        public string ShortCode { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired { get; set; }
        public int ViewCount { get; set; }
    }
}