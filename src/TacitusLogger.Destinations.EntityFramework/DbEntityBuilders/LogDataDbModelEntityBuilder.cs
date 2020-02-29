using Newtonsoft.Json;
using System;
using TacitusLogger.Components.Json;

namespace TacitusLogger.Destinations.EntityFramework
{
    public class LogDbModelEntityBuilder : IDbEntityBuilder
    {
        private static readonly JsonSerializerSettings _defaultJsonSerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private IJsonSerializerFacade _jsonSerializerFacade;

        public LogDbModelEntityBuilder(JsonSerializerSettings jsonSerializerSettings)
        {
            _jsonSerializerSettings = jsonSerializerSettings ?? throw new ArgumentNullException("jsonSerializerSettings");
            _jsonSerializerFacade = new NewtonsoftJsonSerializerFacade();
        }
        public LogDbModelEntityBuilder()
            : this(DefaultJsonSerializerSettings)
        {

        }

        public static JsonSerializerSettings DefaultJsonSerializerSettings => _defaultJsonSerializerSettings;
        public JsonSerializerSettings JsonSerializerSettings => _jsonSerializerSettings;
        public IJsonSerializerFacade JsonSerializerFacade=> _jsonSerializerFacade;

        public Object BuildDbEntity(LogModel logModel)
        {
            return new LogDbModel()
            {
                LogId = logModel.LogId,
                Context = logModel.Context,
                Source = logModel.Source,
                LogType = logModel.LogType,
                Description = logModel.Description,
                TagsJson = _jsonSerializerFacade.Serialize(logModel.Tags, _jsonSerializerSettings),
                LogItemsJson = _jsonSerializerFacade.Serialize(logModel.LogItems, _jsonSerializerSettings),
                LogDate = logModel.LogDate,
            };
        }
        public void ResetJsonSerializerFacade(IJsonSerializerFacade jsonSerializerFacade)
        {
            _jsonSerializerFacade = jsonSerializerFacade ?? throw new ArgumentNullException("jsonSerializerFacade");
        }
    }
}
