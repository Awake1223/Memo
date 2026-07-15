using System.Text.RegularExpressions;
using Memo.Application.DTOs.Tags;
using Memo.Application.Interfaces;
using Memo.Domain.Entities;
using Memo.Domain.Interfaces;

namespace Memo.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly INoteRepository _noteRepository;

        public TagService(ITagRepository tagRepository, INoteRepository noteRepository)
        {
            _tagRepository = tagRepository;
            _noteRepository = noteRepository;
        }

        public async Task<TagEntity?> GetOrCreateTagAsync(string tagName)
        {
            // Нормализуем тег: убираем #, приводим к нижнему регистру
            var normalized = tagName.TrimStart('#').ToLowerInvariant();

            var tag = await _tagRepository.GetByNameAsync(normalized);
            if (tag != null)
                return tag;

            tag = new TagEntity { Name = normalized };
            await _tagRepository.AddAsync(tag);
            await _tagRepository.SaveChangesAsync();

            return tag;
        }

        public async Task ExtractAndSaveTagsAsync(NoteEntity note, string content)
        {
            // Ищем все #теги в тексте
            var regex = new Regex(@"#\w+", RegexOptions.Compiled);
            var matches = regex.Matches(content);

            if (matches.Count == 0)
                return;

            var tagNames = matches
                .Select(m => m.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var tagName in tagNames)
            {
                var tag = await GetOrCreateTagAsync(tagName);
                if (tag != null)
                {
                    // Проверяем, есть ли уже связь
                    var exists = note.NoteTags.Any(nt => nt.TagId == tag.Id);
                    if (!exists)
                    {
                        note.NoteTags.Add(new NoteTagEntity
                        {
                            NoteId = note.Id,
                            TagId = tag.Id,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _noteRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagDto>> GetPopularTagsAsync(int limit = 10)
        {
            var tags = await _tagRepository.GetPopularTagsAsync(limit);
            return tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Count = t.NoteTags?.Count ?? 0 
            });
        }
    }
}