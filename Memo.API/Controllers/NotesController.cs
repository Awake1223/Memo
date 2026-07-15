using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Memo.Application.DTOs.Notes;   
using Memo.Application.Interfaces;    

namespace Memo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto request)
        {
            int? userId = null;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var parsedUserId))
                    userId = parsedUserId;
            }

            // ✅ Передаём ВЕСЬ DTO, а не отдельные поля
            var note = await _noteService.CreateNoteAsync(request, userId);
            return Ok(new { shortCode = note.ShortCode, expiresAt = note.ExpiresAt });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetNote(string shortCode)
        {
            var note = await _noteService.GetNoteByShortCodeAsync(shortCode);
            if (note == null)
                return NotFound(new { message = "Note not found or expired" });

            return Ok(new NoteResponseDto
            {
                ShortCode = note.ShortCode,
                Content = note.Content,
                ExpiresAt = note.ExpiresAt,
                CreatedAt = note.CreatedAt,
                IsExpired = note.ExpiresAt.HasValue && note.ExpiresAt.Value < DateTime.UtcNow,
                ViewCount = note.ViewCount
            });
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotes()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var notes = await _noteService.GetUserNotesAsync(userId);
            return Ok(notes);
        }

        [Authorize]
        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> DeleteNote(string shortCode)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _noteService.DeleteNoteAsync(shortCode, userId);

            if (!result)
                return NotFound(new { message = "Note not found or you don't have permission" });

            return Ok(new { message = "Note deleted successfully" });
        }

        [Authorize]
        [HttpPut("{shortCode}")]
        public async Task<IActionResult> UpdateNote(string shortCode, [FromBody] UpdateNoteDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _noteService.UpdateNoteAsync(shortCode, userId, request.Content);

            if (!result)
                return NotFound(new { message = "Note not found, expired, or you don't have permission" });

            return Ok(new { message = "Note updated successfully" });
        }
    }
}