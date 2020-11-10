using System.Collections.Generic;

namespace TrainTicketMachine.JsonProcessors
{
    public interface IJsonParser
    {
        public List<T> ParseJsonContent<T>(string jsonFileContent);
    }
}
