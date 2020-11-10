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
                throw new GettingDataFromFileException(message, ex);
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
