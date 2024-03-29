﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrainTicketMachine.Models;
using TrainTicketMachine.StationSearch;
using TrainTicketMachine.SystemResponseReceiver;

namespace TrainTicketMachine
{
    public class TrainTicketMachineProcessor : IDisposable
    {
        public void StartProcessing(IServiceProvider serviceProvider)
        {
            InitializeConnectionWithCentralSystem(serviceProvider);
            RunSearchEngine(serviceProvider);
        }

        public void Dispose()
        { }

        private static void InitializeConnectionWithCentralSystem(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var responseSaver = serviceProvider.GetService<SystemResponseSaver>();
            try
            {
                responseSaver.WriteSystemResponseToFileWithRetry(Retries.MaxRetriesNumber);
            }
            catch (ProcessingActionException)
            {
                logger.LogError($"Connection with central system failed after {Retries.MaxRetriesNumber} retries.");
            }
        }

        private static void RunSearchEngine(IServiceProvider serviceProvider)
        {
            var stationsCollector = serviceProvider.GetService<TrainStationsCollector>();
            var specialCharacters = serviceProvider.GetService<SpecialCharacters>();

            List<Station> completeStationsList;
            try
            {
                completeStationsList = stationsCollector.CollectAllTrainStationsFromSystemResponse();
            }
            catch (CollectingDataFromFileException ex)
            {
                Console.WriteLine($"{ex.Message}. Program is closing.");
                return;
            }
            var stationsSelector = new MatchingStationsSelector(completeStationsList);

            var charFromUser = specialCharacters.InitialCharacter;
            while (charFromUser != specialCharacters.ClosingApp)
            {
                DisplayHelper(specialCharacters.ClosingApp);

                charFromUser = Console.ReadKey().KeyChar;
                var userInput = new string(charFromUser.ToString());

                while (!EnteredBreakingChar(charFromUser, specialCharacters))
                {
                    var matchingStations = stationsSelector.GetMatchingStations(userInput);
                    var charsSelector = new MatchingNextCharactersSelector(matchingStations);
                    var nextChars = charsSelector.GetNextChars(userInput);

                    DisplayResults(matchingStations, nextChars, userInput);

                    charFromUser = Console.ReadKey().KeyChar;
                    if (charFromUser == specialCharacters.DeleteCharacter)
                    {
                        userInput.Remove(userInput.Length - 1);
                    }
                    else
                    {
                        userInput += charFromUser;
                    }
                    Console.WriteLine();
                }
            }
        }

        private static bool EnteredBreakingChar(char charFromUser, SpecialCharacters specialCharacters)
        {
            return charFromUser == Environment.NewLine.First() || charFromUser == specialCharacters.ClosingApp;
        }

        private static void DisplayHelper(char closingChar)
        {
            Console.WriteLine("Train Ticket Machine");
            Console.WriteLine($"Type '{closingChar}' to cancel the search and close the window.");
            Console.WriteLine("Press enter to start typing the station name from the beginning.");
            Console.WriteLine("Press backspace to delete the last entered character.");
            Console.WriteLine("Type station name:");
        }

        private static void DisplayResults(IEnumerable<Station> matchingStations, IEnumerable<char> nextChars, string userInput)
        {
            Console.WriteLine("\nMatching train stations:");
            foreach (var matchingStation in matchingStations)
            {
                Console.WriteLine(matchingStation.StationName);
            }

            Console.WriteLine("\nCharacters possible to enter:");
            foreach (var nextChar in nextChars)
            {
                Console.WriteLine(nextChar);
            }

            Console.Write($"\n{userInput}");
        }
    }
}
