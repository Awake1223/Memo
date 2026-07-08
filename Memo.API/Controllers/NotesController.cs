using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Memo.API.DTOs.Notes;
using Memo.API.Services;
using System.Security.Claims;

namespace Memo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly NoteService _noteService;
        private readonly AuthService _authService;

        public NotesController(NoteService noteService, AuthService authService)
        {
            _noteService = noteService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int? userId = null;

            // Если пользователь авторизован, привязываем заметку к нему
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out var parsedUserId))
                    userId = parsedUserId;
            }

            var note = await _noteService.CreateNoteAsync(request.Content, request.Lifetime, userId);
            return Ok(new { shortCode = note.ShortCode, expiresAt = note.ExpiresAt });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetNote(string shortCode)
        {
            var note = await _noteService.GetNoteByShortCodeAsync(shortCode);
            if (note == null)
                return NotFound(new { message = "Note not found or expired" });

            return Ok(new NoteResponse
            {
                ShortCode = note.ShortCode,
                Content = note.Content,
                ExpiresAt = note.ExpiresAt,
                CreatedAt = note.CreatedAt,
                IsExpired = note.ExpiresAt.HasValue && note.ExpiresAt.Value < DateTime.UtcNow
            });
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotes()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var notes = await _noteService.GetUserNotesAsync(userId);
            var response = notes.Select(n => new NoteResponse
            {
                ShortCode = n.ShortCode,
                Content = n.Content.Length > 100 ? n.Content[..100] + "..." : n.Content,
                ExpiresAt = n.ExpiresAt,
                CreatedAt = n.CreatedAt,
                IsExpired = n.ExpiresAt.HasValue && n.ExpiresAt.Value < DateTime.UtcNow
            });

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("{shortCode}")]
        public async Task<IActionResult> DeleteNote(string shortCode)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            var result = await _noteService.DeleteNoteAsync(shortCode, userId);
            if (!result)
                return NotFound(new { message = "Note not found or you don't have permission" });

            return Ok(new { message = "Note deleted successfully" });
        }
    }
}