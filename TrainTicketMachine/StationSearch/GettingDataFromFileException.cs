using System;

namespace TrainTicketMachine.StationSearch
{
    public class GettingDataFromFileException : Exception
    {
        public GettingDataFromFileException(string message) : base(message)
        {
        }

        public GettingDataFromFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
