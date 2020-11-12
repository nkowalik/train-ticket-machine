using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TrainTicketMachine.JsonProcessors
{
    public class JsonParser : IJsonParser
    {
        private readonly ILogger _logger;

        public JsonParser(ILogger<JsonParser> logger)
        {
            _logger = logger;
        }

        public List<T> ParseJsonContent<T>(string jsonFileContent)
        {
            try
            {
                var items = JsonConvert.DeserializeObject<List<T>>(jsonFileContent);
                return items;
            }
            catch (JsonSerializationException)
            {
                var message = $"Json file content is invalid:\n{jsonFileContent}";
                _logger.LogError(message);
                throw;
            }
        }
    }
}
