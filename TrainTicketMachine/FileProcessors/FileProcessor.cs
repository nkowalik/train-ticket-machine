using System.IO;
using System.Net;
using Microsoft.Extensions.Logging;

namespace TrainTicketMachine.FileProcessors
{
    public class FileProcessor : IFileProcessor
    {
        private readonly ILogger _logger;

        public FileProcessor(ILogger<FileProcessor> logger)
        {
            _logger = logger;
        }

        public bool WriteWebsiteContentToFile(string uri, string fullPathToFile)
        {
            using var client = new WebClient();
            var websiteContent = client.DownloadString(uri);
            File.WriteAllText(fullPathToFile, websiteContent);
            return true;
        }

        public string ReadFile(string pathToFile)
        {
            try
            {
                using var reader = new StreamReader(pathToFile);
                var fileContent = reader.ReadToEnd();
                return fileContent;
            }
            catch (FileNotFoundException ex)
            {
                var message = $"Invalid path to file: {pathToFile}";
                _logger.LogError(message);
                throw new InvalidPathException(message, ex);
            }
        }
    }
}
