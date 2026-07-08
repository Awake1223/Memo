namespace Memo.API.DTOs.Notes
{
    public class NoteResponse
    {
        public string ShortCode { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired { get; set; }
    }
}