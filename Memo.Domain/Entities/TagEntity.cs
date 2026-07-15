namespace Memo.Domain.Entities
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<NoteTagEntity> NoteTags { get; set; } = new();
    }
}