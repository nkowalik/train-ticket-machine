using System;
using System.Collections.Generic;
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
            var serviceProvider = services.BuildServiceProvider();

            GetResponseFromCentralSystem(serviceProvider);

            RunSearchEngine(serviceProvider);
        }

        private static void GetResponseFromCentralSystem(IServiceProvider serviceProvider)
        {
            var responseSaver = serviceProvider.GetService<SystemResponseSaver>();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                responseSaver.WriteSystemResponseToFileWithRetry(Retries.MaxRetriesNumber);
            }
            catch (CentralSystemConnectionException)
            {
                logger.LogError($"Connection with central system failed after {Retries.MaxRetriesNumber} retries.");
            }
        }

        private static void RunSearchEngine(IServiceProvider serviceProvider)
        {
            var stationsCollector = serviceProvider.GetService<TrainStationsCollector>();
            var specialCharacters = serviceProvider.GetService<SpecialCharacters>();

            var completeStationsList = stationsCollector.CollectAllTrainStationsFromSystemResponse();
            var stationsSelector = new MatchingStationsSelector(completeStationsList);
            
            var charFromUser = specialCharacters.InitialCharacter;
            while (charFromUser != specialCharacters.ClosingApp)
            {
                DisplayHelper(specialCharacters.ClosingApp);

                charFromUser = Console.ReadKey().KeyChar;
                var userInput = new string(charFromUser.ToString());

                while (charFromUser != Environment.NewLine.First() && charFromUser != specialCharacters.ClosingApp)
                {
                    var matchingStations = stationsSelector.GetMatchingStations(userInput);
                    var charsSelector = new MatchingNextCharactersSelector(matchingStations);
                    var nextChars = charsSelector.GetNextChars(userInput);

                    DisplayResults(matchingStations, nextChars, userInput);

                    charFromUser = Console.ReadKey().KeyChar;
                    userInput += charFromUser;
                    Console.WriteLine();
                }
            }
        }

        private static void DisplayHelper(char closingChar)
        {
            Console.WriteLine("Train Ticket Machine");
            Console.WriteLine($"Type '{closingChar}' to cancel the search and close the window.");
            Console.WriteLine("Type station name:");
        }

        private static void DisplayResults(IEnumerable<Station> matchingStations, IEnumerable<char> nextChars, string userInput)
        {
            foreach (var matchingStation in matchingStations)
            {
                Console.WriteLine(matchingStation.StationName);
            }

            foreach (var nextChar in nextChars)
            {
                Console.WriteLine(nextChar);
            }

            Console.Write($"\n{userInput}");
        }
    }
}
