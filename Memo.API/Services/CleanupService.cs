using Memo.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Memo.API.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupService> _logger;

        public CleanupService(IServiceProvider serviceProvider, ILogger<CleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<MemoDbContext>();

                    var expiredNotes = context.Notes
                        .Where(n => n.ExpiresAt != null && n.ExpiresAt < DateTime.UtcNow && !n.IsDeleted);

                    var count = await expiredNotes.CountAsync(stoppingToken);
                    if (count > 0)
                    {
                        await expiredNotes.ExecuteUpdateAsync(
                            n => n.SetProperty(x => x.IsDeleted, true),
                            stoppingToken
                        );
                        _logger.LogInformation($"Cleaned up {count} expired notes");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cleanup");
                }

                // Проверяем каждую минуту
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}