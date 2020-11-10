using System;

namespace TrainTicketMachine.SystemResponseReceiver
{
    public class CentralSystemConnectionException : Exception
    {
        public CentralSystemConnectionException(string message) : base(message)
        {
        }

        public CentralSystemConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
