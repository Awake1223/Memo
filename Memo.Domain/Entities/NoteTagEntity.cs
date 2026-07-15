namespace Memo.Domain.Entities
{
    public class NoteTagEntity
    {
        public int NoteId { get; set; }
        public int TagId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public NoteEntity Note { get; set; } = null!;
        public TagEntity Tag { get; set; } = null!;
    }
}