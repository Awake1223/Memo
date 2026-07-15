namespace Memo.Application.DTOs.Notes
{
    public class CreateNoteDto
    {
        public string Content { get; set; } = string.Empty;
        public NoteLifetime Lifetime { get; set; }
    }

    public enum NoteLifetime
    {
        OneHour = 1,
        OneDay = 2,
        Forever = 3
    }
}