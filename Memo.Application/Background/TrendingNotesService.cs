using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Memo.Domain.Interfaces;

namespace Memo.Application.Background
{
    public class TrendingNotesService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TrendingNotesService> _logger;
        private readonly Dictionary<string, int> _trendingCache = new();

        public TrendingNotesService(
            IServiceProvider serviceProvider,
            ILogger<TrendingNotesService> logger)
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
                    var noteRepository = scope.ServiceProvider
                        .GetRequiredService<INoteRepository>();

                    var trending = await noteRepository.GetTrendingNotesAsync(10);

                    _trendingCache.Clear();
                    foreach (var note in trending)
                    {
                        _trendingCache[note.ShortCode] = note.ViewCount;
                    }

                    _logger.LogInformation($"Trending notes updated. Found {trending.Count()} hot notes.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating trending notes");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        public IReadOnlyDictionary<string, int> GetTrendingNotes()
        {
            return _trendingCache;
        }
    }
}