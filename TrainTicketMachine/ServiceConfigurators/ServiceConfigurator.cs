using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using TrainTicketMachine.FileProcessors;
using TrainTicketMachine.JsonProcessors;
using TrainTicketMachine.Models;
using TrainTicketMachine.StationSearch;
using TrainTicketMachine.SystemResponseReceiver;

namespace TrainTicketMachine.ServiceConfigurators
{
    internal class ServiceConfigurator
    {
        internal static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var logger = CreateInternalLogger(configuration);

            ConfigureLogging(services, configuration, logger);
            ConfigureAppSettings(services, configuration, logger);
            ConfigureProcessorsDependencies(services, logger);

            var systemResponsePaths = configuration.GetSection("CentralSystemResponse").Get<SystemResponse>();
            ConfigureTrainStationsCollectorDependencies(services, logger, systemResponsePaths);
            ConfigureCentralSystemResponseDependencies(services, logger, systemResponsePaths);

            return services;
        }

        private static ILogger CreateInternalLogger(IConfiguration configuration)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddNLog(configuration);
                }
            );

            return loggerFactory.CreateLogger<Program>();
        }

        private static void ConfigureLogging(IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            logger.LogDebug("Configuring Logging dependencies...");
            
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                loggingBuilder.AddNLog(configuration);
            });
        }

        private static void ConfigureAppSettings(IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            logger.LogDebug("Configuring application settings...");

            var specialChars = configuration.GetSection("SpecialCharacters").Get<SpecialCharacters>();
            services.AddSingleton(serviceProvider => specialChars);

            var retries = configuration.GetSection("Retries").Get<Retries>();
            services.AddSingleton(serviceProvider => retries);
        }

        private static void ConfigureProcessorsDependencies(IServiceCollection services, ILogger logger)
        {
            logger.LogDebug("Configuring file and json processors dependencies...");

            services.AddSingleton<IJsonParser>(serviceProvider =>
                new JsonParser(serviceProvider.GetService<ILogger<JsonParser>>()));
            services.AddSingleton<IFileProcessor>(serviceProvider => 
                new FileProcessor(serviceProvider.GetService<ILogger<FileProcessor>>()));
        }

        private static void ConfigureTrainStationsCollectorDependencies(IServiceCollection services, ILogger logger, 
            SystemResponse systemResponsePaths)
        {
            logger.LogDebug("Configuring TrainStationsCollector dependencies...");

            services.AddSingleton(serviceProvider => new TrainStationsCollector(
                serviceProvider.GetService<IJsonParser>(),
                serviceProvider.GetService<IFileProcessor>(),
                systemResponsePaths.PathToResponseFile
            ));
        }

        private static void ConfigureCentralSystemResponseDependencies(IServiceCollection services, ILogger logger, 
            SystemResponse systemResponsePaths)
        {
            logger.LogDebug("Configuring central system dependencies...");

            services.AddSingleton(serviceProvider => systemResponsePaths);

            services.AddSingleton(serviceProvider => new SystemResponseSaver(
                serviceProvider.GetService<IFileProcessor>(),
                systemResponsePaths.SystemResponseUri,
                systemResponsePaths.PathToResponseFile,
                serviceProvider.GetService<ILogger<SystemResponseSaver>>()
            ));
        }
    }
}
