using System.Linq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TrainTicketMachine.FileProcessors;
using TrainTicketMachine.JsonProcessors;
using TrainTicketMachine.StationSearch;
using Xunit;

namespace TrainTicketMachine.UnitTests.StationSearch
{
    public class TrainStationsCollectorTests
    {
        private readonly FileProcessor _fileProcessor;
        private readonly JsonParser _jsonParser;

        public TrainStationsCollectorTests()
        {
            var fileProcessorLogger = Substitute.For<ILogger<FileProcessor>>();
            var jsonParserLogger = Substitute.For<ILogger<JsonParser>>();

            _fileProcessor = new FileProcessor(fileProcessorLogger);
            _jsonParser = new JsonParser(jsonParserLogger);
        }

        [Fact]
        public void CollectAllTrainStationsFromSystemResponseSucceeded()
        {
            const string expectedStationName = "Abbey Wood";
            const string pathToFile = @".\Resources\test.json";

            var collector = new TrainStationsCollector(_jsonParser, _fileProcessor, pathToFile);
            var stations = collector.CollectAllTrainStationsFromSystemResponse();

            Assert.NotEmpty(stations);
            Assert.Equal(expectedStationName, stations.First().StationName);
        }

        [Fact]
        public void CollectAllTrainStationsFromSystemResponseFailedWithInvalidPathToFile()
        {
            const string invalidPath = @".\invalid_path";

            var collector = new TrainStationsCollector(_jsonParser, _fileProcessor, invalidPath);

            Assert.Throws<CollectingDataFromFileException>(() =>
                collector.CollectAllTrainStationsFromSystemResponse());
        }
    }
}
