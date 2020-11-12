using System;
using System.Collections.Generic;
using TrainTicketMachine.FileProcessors;
using TrainTicketMachine.JsonProcessors;
using TrainTicketMachine.Models;

namespace TrainTicketMachine.StationSearch
{
    public class TrainStationsCollector
    {
        public IFileProcessor FileProcessor { get; }
        public IJsonParser JsonParser { get; }
        public string PathToFileWithSystemResponse { get; }

        public TrainStationsCollector(IJsonParser jsonParser, IFileProcessor fileProcessor, string pathToFile)
        {
            JsonParser = jsonParser;
            FileProcessor = fileProcessor;
            PathToFileWithSystemResponse = pathToFile;
        }

        /// <summary>
        /// Reads content from file with saved system response and converts it to the collection of Station objects.
        /// </summary>
        /// <returns>List of <see cref="Station"/> instances that represents all train stations that are present in the central system.</returns>
        public List<Station> CollectAllTrainStationsFromSystemResponse()
        {
            try
            {
                var stations = CollectAllTrainStations(PathToFileWithSystemResponse);
                return stations;
            }
            catch (Exception ex)
            {
                var message = $"Collecting central system data from file '{PathToFileWithSystemResponse}' failed.";
                throw new CollectingDataFromFileException(message, ex);
            }
        }

        private List<Station> CollectAllTrainStations(string pathToFile)
        {
            var fileContent = FileProcessor.ReadFile(pathToFile);
            var allStations = JsonParser.ParseJsonContent<Station>(fileContent);

            return allStations;
        }
    }
}
