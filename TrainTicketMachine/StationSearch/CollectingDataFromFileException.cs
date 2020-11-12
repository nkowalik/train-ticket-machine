using System;

namespace TrainTicketMachine.StationSearch
{
    /// <summary>
    /// The exception that is thrown when collecting data from files fails.
    /// </summary>
    public class CollectingDataFromFileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CollectingDataFromFileException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">A <see cref="string"/> that represents the message of the exception.</param>
        public CollectingDataFromFileException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CollectingDataFromFileException"/> class with a specified error message and a reference to an inner exception that is the cause of the exception.
        /// </summary>
        /// <param name="message">A <see cref="string"/> that represents the message of the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/> instance that represents the inner exception that is the primary cause of the exception.</param>
        public CollectingDataFromFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
