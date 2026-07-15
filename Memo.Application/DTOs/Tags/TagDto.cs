namespace Memo.Application.DTOs.Tags
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; } // сколько заметок с этим тегом
    }
}