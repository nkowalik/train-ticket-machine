using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrainTicketMachine.ServiceConfigurators;

namespace TrainTicketMachine
{
    public class TrainTicketMachineService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var services = ServiceConfigurator.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("Program started.");

            try
            {
                using var trainTicketMachineProcessor = serviceProvider.GetService<TrainTicketMachineProcessor>();
                trainTicketMachineProcessor.StartProcessing(serviceProvider);
                stoppingToken.WaitHandle.WaitOne();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was a problem while setting up the service.");
                logger.LogTrace(ex, ex.StackTrace);
            }

            logger.LogInformation("Program ended.");

            return Task.CompletedTask;
        }
    }
}
