using System;
using Microsoft.Extensions.Logging;
using TrainTicketMachine.FileProcessors;

namespace TrainTicketMachine.SystemResponseReceiver
{
    public class SystemResponseSaver
    {
        public IFileProcessor FileProcessor { get; }
        public string PathToFileWithSystemResponse { get; }
        public string Uri { get; }

        private readonly ILogger _logger;

        public SystemResponseSaver(IFileProcessor fileProcessor, 
            string uri, 
            string pathToFile,
            ILogger<SystemResponseSaver> logger)
        {
            FileProcessor = fileProcessor;
            Uri = uri;
            PathToFileWithSystemResponse = pathToFile;
            _logger = logger;
        }

        /// <summary>
        /// Tries to write all content received by the central system to a file with the retries on failure.
        /// </summary>
        /// <param name="retries">The <see cref="int"/> that represents the maximum number of retries to write response to the file after failure or loss of connection to the system.</param>
        public void WriteSystemResponseToFileWithRetry(int retries)
        {
            if (retries < 1)
            {
                throw new ArgumentException("Number of retries must be greater than 0.");
            }

            const string errorMessage = "Writing system response to file failed.";
            try
            {
                WriteSystemResponseToFileWithRetry(WriteSystemResponseToFile, errorMessage, retries);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw new CentralSystemConnectionException($"{ex.Message}");
            }
        }

        private static void WriteSystemResponseToFileWithRetry(Action writeToFileAction, string errorMessage, int retries)
        {
            Exception lastException = null;

            while (retries > 0)
            {
                try
                {
                    writeToFileAction.Invoke();
                    return;
                }
                catch (Exception ex)
                {
                    retries--;
                    lastException = ex;
                }
            }

            throw lastException == null
                ? new CentralSystemConnectionException(errorMessage)
                : new CentralSystemConnectionException(errorMessage, lastException);
        }

        private void WriteSystemResponseToFile()
        {
            try
            {
                FileProcessor.WriteWebsiteContentToFile(Uri, PathToFileWithSystemResponse);
            }
            catch (Exception ex)
            {
                var message = $"Writing system response to file '{PathToFileWithSystemResponse}' failed.";
                _logger.LogError(message);
                throw new CentralSystemConnectionException(message, ex);
            }
        }
    }
}
