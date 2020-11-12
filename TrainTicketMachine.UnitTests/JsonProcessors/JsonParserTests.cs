using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using TrainTicketMachine.JsonProcessors;
using TrainTicketMachine.Models;
using Xunit;

namespace TrainTicketMachine.UnitTests.JsonProcessors
{
    public class JsonParserTests
    {
        private readonly ILogger<JsonParser> _logger;

        public JsonParserTests()
        {
            _logger = Substitute.For<ILogger<JsonParser>>();
        }

        [Fact]
        public void ReadFromFileSucceeded()
        {
            const string testStationName = "Abbey Wood";
            const string jsonFileContent = "[{\"stationCode\": \"ABW\", \"stationName\": \"Abbey Wood\"}]";

            var processor = new JsonParser(_logger);
            var stations = processor.ParseJsonContent<Station>(jsonFileContent);

            Assert.Collection(stations, station => 
                Assert.Contains(testStationName, station.StationName));
        }

        [Fact]
        public void ReadFromFileFailedWithInvalidJsonFileContentException()
        {
            const string invalidJsonFileContent = "{\"stationCode\": \"ABW\", \"stationName\": \"Abbey Wood\"}";

            var processor = new JsonParser(_logger);

            Assert.Throws<JsonSerializationException>(() =>
                processor.ParseJsonContent<Station>(invalidJsonFileContent));
        }
    }
}
