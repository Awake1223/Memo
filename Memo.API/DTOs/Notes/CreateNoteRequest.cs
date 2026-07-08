using System.ComponentModel.DataAnnotations;

namespace Memo.API.DTOs.Notes
{
    public class CreateNoteRequest
    {
        [Required, MaxLength(10000)]
        public string Content { get; set; } = string.Empty;

        public NoteLifetime Lifetime { get; set; } = NoteLifetime.Forever;
    }

    public enum NoteLifetime
    {
        OneHour = 1,
        OneDay = 2,
        Forever = 3
    }
}