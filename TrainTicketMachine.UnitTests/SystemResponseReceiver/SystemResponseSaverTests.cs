using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TrainTicketMachine.FileProcessors;
using TrainTicketMachine.SystemResponseReceiver;
using Xunit;

namespace TrainTicketMachine.UnitTests.SystemResponseReceiver
{
    public class SystemResponseSaverTests
    {
        private readonly IFileProcessor _fileProcessor;
        private readonly ILogger<SystemResponseSaver> _logger;

        public SystemResponseSaverTests()
        {
            _fileProcessor = Substitute.For<IFileProcessor>();
            _logger = Substitute.For<ILogger<SystemResponseSaver>>();
        }
        
        [Fact]
        public void WriteSystemResponseToFileWithRetrySucceededAfter2Retries()
        {
            _fileProcessor.WriteWebsiteContentToFile(Arg.Any<string>(), Arg.Any<string>()).Returns(
                x => throw new ApplicationException(),
                x => throw new ApplicationException(),
                x => true);

            var systemResponseSaver = new SystemResponseSaver(_fileProcessor, string.Empty, string.Empty, _logger);
            systemResponseSaver.WriteSystemResponseToFileWithRetry(3);
        }

        [Fact]
        public void WriteSystemResponseToFileWithRetryFailedAfter3Retries()
        {
            _fileProcessor.WriteWebsiteContentToFile(Arg.Any<string>(), Arg.Any<string>()).Throws(new ApplicationException());

            var systemResponseSaver = new SystemResponseSaver(_fileProcessor, string.Empty, string.Empty, _logger);

            Assert.Throws<CentralSystemConnectionException>(() =>
                systemResponseSaver.WriteSystemResponseToFileWithRetry(3));
        }

        [Fact]
        public void RetriesNumberMustBeGreaterThanZero()
        {
            var systemResponseSaver = new SystemResponseSaver(_fileProcessor, string.Empty, string.Empty, _logger);

            Assert.Throws<ArgumentException>(() => systemResponseSaver.WriteSystemResponseToFileWithRetry(0));
        }
    }
}
