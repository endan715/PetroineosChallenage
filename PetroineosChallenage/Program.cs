using CsvHelper;
using PetroineosChallenage;
using PetroineosChallenage.Settings;
using Polly;
using Services;
using System.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, option) =>
    {
        option.ValidateOnBuild = true;
    })
    .ConfigureAppConfiguration(config =>
    {

    })
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration config = hostContext.Configuration;
        var outputSettings = GetAppSettings<OutputSettings>(config, OutputSettings.SettingSectionName);
        var retrySettings = GetAppSettings<RetrySettings>(config, RetrySettings.SettingSectionName);
        var workerSettings = GetAppSettings<WorkerSettings>(config, WorkerSettings.SettingSectionName);
        services.AddSingleton(outputSettings);
        services.AddSingleton(retrySettings);
        services.AddSingleton(workerSettings);
        services.AddSingleton<IDataWriter, CSVDataWriter>();
        services.AddSingleton<IRetryPolicy, RetryPolicy>();
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<ITradePositionAggregator, TradePositionAggregator>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

static T GetAppSettings<T>(IConfiguration config, string sectionName)
{
    IConfigurationSection section = config.GetSection(sectionName);
    if (!section.Exists())
    {
        throw new ConfigurationErrorsException($"Configuration section ${sectionName} does not exist");
    }

    T settings = section.Get<T>();
    return settings;
}
