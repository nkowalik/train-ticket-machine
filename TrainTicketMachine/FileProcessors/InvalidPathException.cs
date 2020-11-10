﻿using System;

namespace TrainTicketMachine.FileProcessors
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException(string message) : base(message)
        {
        }

        public InvalidPathException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
