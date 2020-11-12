using System;
using TrainTicketMachine.SystemResponseReceiver;

namespace TrainTicketMachine.Utils
{
    public static class RetryingHandler
    {
        /// <summary>
        /// Processes the received action until it succeeds or exceeds the number of retries.
        /// </summary>
        /// <param name="actionToProcess"><see cref="Action"/> instance that represents an action to be processed.</param>
        /// <param name="errorMessage">A <see cref="string"/> that represents the message that is passed to the exception.</param>
        /// <param name="retries">An <see cref="int"/> that represents the maximum number of retries to process the action.</param>
        public static void ProcessActionWithRetry(Action actionToProcess, string errorMessage, int retries)
        {
            Exception lastException = null;

            while (retries > 0)
            {
                try
                {
                    actionToProcess.Invoke();
                    return;
                }
                catch (Exception ex)
                {
                    retries--;
                    lastException = ex;
                }
            }

            throw lastException == null
                ? new ProcessingActionException(errorMessage)
                : new ProcessingActionException(errorMessage, lastException);
        }
    }
}
