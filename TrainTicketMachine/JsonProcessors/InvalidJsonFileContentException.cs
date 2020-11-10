using System;

namespace TrainTicketMachine.JsonProcessors
{
    public class InvalidJsonFileContentException : Exception
    {
        public InvalidJsonFileContentException(string message) : base(message)
        {
        }

        public InvalidJsonFileContentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
