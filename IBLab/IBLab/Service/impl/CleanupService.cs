using IBLab.Repository.Interfaces;
using IBLab.Service.interfaces;

namespace IBLab.Service.impl
{
    public class CleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(4);

        public CleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ExecuteCleanupAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_cleanupInterval, stoppingToken);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var tempUserRepository = scope.ServiceProvider.GetRequiredService<ITempUserRepository>();
                        await tempUserRepository.CleanupExpiredTempUsers();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during cleanup: {ex.Message}");
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ExecuteCleanupAsync(stoppingToken);
        }
    }
}

