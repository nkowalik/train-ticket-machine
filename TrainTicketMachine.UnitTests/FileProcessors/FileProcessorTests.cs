using System.IO;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TrainTicketMachine.FileProcessors;
using Xunit;

namespace TrainTicketMachine.UnitTests.FileProcessors
{
    public class FileProcessorTests
    {
        private readonly ILogger<FileProcessor> _logger;

        public FileProcessorTests()
        {
            _logger = Substitute.For<ILogger<FileProcessor>>();
        }

        [Fact]
        public void ReadFromFileSucceeded()
        {
            const string pathToFile = @".\Resources\test.json";
            const string expectedFileContent = "[{\"stationCode\": \"ABW\", \"stationName\": \"Abbey Wood\"}]";

            var processor = new FileProcessor(_logger);
            var fileContent = processor.ReadFile(pathToFile);

            Assert.Equal(expectedFileContent, fileContent);
        }

        [Fact]
        public void ReadFromFileFailedWithInvalidPathException()
        {
            const string invalidPathToFile = @".\Resources\invalidName.json";

            var processor = new FileProcessor(_logger);

            var ex = Assert.Throws<InvalidPathException>(() => processor.ReadFile(invalidPathToFile));
            Assert.IsType<FileNotFoundException> (ex.InnerException);
        }
    }
}
