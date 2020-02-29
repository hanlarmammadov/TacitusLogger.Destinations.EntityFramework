using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace TacitusLogger.Destinations.EntityFramework.TestHelpers
{
    public static class LogDbModelAsserts
    {
        public static void AssertLogModelModelIsEqualToLogModel(LogDbModel LogDbModel, LogModel logData, JsonSerializerSettings jsonSerializerSettings)
        {
            Assert.AreEqual(logData.LogId, LogDbModel.LogId);
            Assert.AreEqual(logData.Context, LogDbModel.Context);
            Assert.AreEqual(logData.LogType, LogDbModel.LogType);
            Assert.AreEqual(logData.Source, LogDbModel.Source);
            Assert.AreEqual(logData.LogDate, LogDbModel.LogDate);
            Assert.AreEqual(logData.Description, LogDbModel.Description);
            Assert.AreEqual(JsonConvert.SerializeObject(logData.LogItems, jsonSerializerSettings), LogDbModel.LogItemsJson);
            Assert.AreEqual(JsonConvert.SerializeObject(logData.Tags, jsonSerializerSettings), LogDbModel.TagsJson);
        }
        public static void AssertLogModelModelIsEqualToLogModel(LogDbModel LogDbModel, LogModel logData)
        {
            AssertLogModelModelIsEqualToLogModel(LogDbModel, logData, new JsonSerializerSettings());
        }
    }
}
