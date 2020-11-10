using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainTicketMachine.Models;
using TrainTicketMachine.ServiceConfigurators;
using TrainTicketMachine.StationSearch;
using TrainTicketMachine.SystemResponseReceiver;

namespace TrainTicketMachine
{
    internal class Program
    {
        private static void Main()
        {
            var services = ServiceConfigurator.ConfigureServices();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("Program started.");

            var responseSaver = serviceProvider.GetService<SystemResponseSaver>();
            responseSaver.WriteSystemResponseToFileWithRetry(Retries.MaxRetriesNumber);

            var stationsCollector = serviceProvider.GetService<TrainStationsCollector>();
            var fullStationsList = stationsCollector.CollectAllTrainStationsFromSystemResponse();
            var stationsSelector = new MatchingStationsSelector(fullStationsList);

            var specialCharacters = serviceProvider.GetService<SpecialCharacters>();
            var nextCharFromUser = specialCharacters.InitialCharacter;
            while (nextCharFromUser != specialCharacters.ClosingApp)
            {
                Console.WriteLine($"Train Ticket Machine\nType '{specialCharacters.ClosingApp}' to cancel the search and close the window.");
                Console.WriteLine("Type station name:");
                nextCharFromUser = Console.ReadKey().KeyChar;
                var userInput = new string(nextCharFromUser.ToString());

                while (nextCharFromUser != Environment.NewLine.First() && nextCharFromUser != specialCharacters.ClosingApp)
                {
                    var matchingStations = stationsSelector.GetMatchingStations(userInput);
                    var charsSelector = new MatchingNextCharactersSelector(matchingStations);
                    var nextChars = charsSelector.GetNextChars(userInput);

                    foreach (var matchingStation in matchingStations)
                    {
                        Console.WriteLine(matchingStation.StationName);
                    }

                    foreach (var nextChar in nextChars)
                    {
                        Console.WriteLine(nextChar);
                    }

                    Console.Write($"\n{userInput}");
                    nextCharFromUser = Console.ReadKey().KeyChar;
                    userInput += nextCharFromUser;
                    Console.WriteLine();
                }
            }
        }
    }
}
