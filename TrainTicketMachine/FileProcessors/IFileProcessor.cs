namespace TrainTicketMachine.FileProcessors
{
    public interface IFileProcessor
    {
        /// <summary>
        /// Writes all content received by the central system to a file.
        /// </summary>
        /// <param name="uri">A <see cref="string"/> that represents a link to the location from which the content of the central system's response is retrieved.</param>
        /// <param name="fullPathToFile">A <see cref="string"/> that represents the full path to the file where the central system response content will be saved.</param>
        /// <returns>A <see cref="bool"/> value that indicates whether the file was successfully written.</returns>
        public bool WriteWebsiteContentToFile(string uri, string fullPathToFile);

        /// <summary>
        /// Reads the file content and saves it as a <see cref="string"/>.
        /// </summary>
        /// <param name="fullPathToFile">A string that represents the full path to the file where the central system response content is saved.</param>
        /// <returns>A <see cref="string"/> that represents the entire content of the file.</returns>
        public string ReadFile(string fullPathToFile);
    }
}
