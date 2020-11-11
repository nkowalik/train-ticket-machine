using System.Collections.Generic;

namespace TrainTicketMachine.JsonProcessors
{
    public interface IJsonParser
    {
        /// <summary>
        /// Deserializes the received content to a list of elements of the specified type.
        /// </summary>
        /// <typeparam name="T">The element type of the list</typeparam>
        /// <param name="jsonFileContent">A <see cref="string"/> that represents the content of the file in JSON format.</param>
        /// <returns></returns>
        public List<T> ParseJsonContent<T>(string jsonFileContent);
    }
}
