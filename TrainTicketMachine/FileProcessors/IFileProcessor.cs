namespace TrainTicketMachine.FileProcessors
{
    public interface IFileProcessor
    {
        public bool WriteWebsiteContentToFile(string uri, string fullPathToFile);
        public string ReadFile(string pathToFile);
    }
}
