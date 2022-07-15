using CsvHelper;
using PetroineosChallenage.Settings;
using Services;

namespace PetroineosChallenage
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPowerService _powerService;
        private readonly ITradePositionAggregator _tradePositionAggregator;
        private readonly IRetryPolicy _retryPolicy;
        private readonly IDataWriter _dataWriter;
        private readonly WorkerSettings _workerSettings;

        public Worker(ILogger<Worker> logger, IPowerService powerService, ITradePositionAggregator tradePositionAggregator, IRetryPolicy retryPolicy, IDataWriter dataWriter, WorkerSettings workerSettings)
        {
            _logger = logger;
            _powerService = powerService;
            _tradePositionAggregator = tradePositionAggregator;
            _retryPolicy = retryPolicy;
            _dataWriter = dataWriter;
            _workerSettings = workerSettings;
            _logger.LogInformation("Service starting...");
            _logger.LogInformation($"Current worker settings {_workerSettings.ToJsonString()}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                //first run
                await RunExtractAsync(stoppingToken);

                var timer = new PeriodicTimer(TimeSpan.FromMinutes(_workerSettings.Interval));

                //subsequent run will be trigger by timer
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await RunExtractAsync(stoppingToken);
                }
            }
            catch(OperationCanceledException ex)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Service stop requested, shutting down service");
                }
                else
                {
                    _logger.LogError(ex, "Unexpected operation cancellation request received, shutting down service");
                }
            }  
        }

        private async Task RunExtractAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Data export running at UTC: {time}", DateTime.UtcNow);
            var trades = await _retryPolicy.ExecuteAsync<IEnumerable<PowerTrade>>(ct => _powerService.GetTradesAsync(DateTime.Now), stoppingToken);
            var positions = _tradePositionAggregator.GetAggregatedTradePositions(trades);
            await _dataWriter.WriteAsync<HourlyTradePosition>(positions);
        }
    }
}