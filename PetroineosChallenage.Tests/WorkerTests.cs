using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using PetroineosChallenage.Settings;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage.Tests
{
    public class WorkerTests
    {
            
        [Fact]
        public async void Worker_Should_Run_Data_Export_Four_Times_In_Ten_Seconds()
        {
            var workerSettings = new WorkerSettings { Interval = 0.05 }; // 3 seconds
            var outputSettings = new OutputSettings { Location = "C:\\Temp" };
            var aggregatorSettings = new AggregatorSettings { StartHour = 23 };

            var workerLoggerMock = new Mock<ILogger<Worker>>();
            var powerServiceMock = new Mock<IPowerService>();
            var aggregatorMock = new Mock<ITradePositionAggregator>();
            var retryPolicyMock = new Mock<IRetryPolicy>();
            var dataWriterMock = new Mock<IDataWriter>();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IHostedService, Worker>();
            services.AddSingleton<ILogger<Worker>>(workerLoggerMock.Object);
            services.AddSingleton(outputSettings);
            services.AddSingleton(aggregatorSettings);
            services.AddSingleton(retryPolicyMock.Object);
            services.AddSingleton(workerSettings);
            services.AddSingleton(dataWriterMock.Object);
            services.AddSingleton(powerServiceMock.Object);
            services.AddSingleton(aggregatorMock.Object);

            var serviceProvider = services.BuildServiceProvider();
            var hostedService = serviceProvider.GetService<IHostedService>();

            //Act
            await hostedService.StartAsync(CancellationToken.None);


            await Task.Delay(10000);//Give 10s to invoke the methods under test

            await hostedService.StopAsync(CancellationToken.None);

            //interval 3 seconds, runtime 10 seconds, it should result 4 api calls
            aggregatorMock.Verify(a => a.GetAggregatedTradePositions(It.IsAny<IEnumerable<PowerTrade>>()), Times.Exactly(4));
            dataWriterMock.Verify(a => a.WriteAsync(It.IsAny<IEnumerable<HourlyTradePosition>>()), Times.Exactly(4));
        }
    }
}
