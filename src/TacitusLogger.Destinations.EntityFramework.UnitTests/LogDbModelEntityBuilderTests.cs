using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using TacitusLogger.Components.Json;
using TacitusLogger.Destinations.EntityFramework.TestHelpers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests
{
    [TestFixture]
    public class LogDbModelEntityBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_JsonSerializerSettings_When_Called_Sets_JsonSerializerSettings()
        {
            // Arrange
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder(jsonSerializerSettings);

            // Assert
            Assert.AreEqual(jsonSerializerSettings, logDbModelEntityBuilder.JsonSerializerSettings);
        }

        [Test]
        public void Ctor_Taking_JsonSerializerSettings_When_Called_With_Null_JsonSerializerSettings()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder(null as JsonSerializerSettings);
            });
        }

        [Test]
        public void Ctor_Default_When_Called_Sets_Default_JsonSerializerSettings()
        {
            // Act
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();

            // Assert
            Assert.AreEqual(LogDbModelEntityBuilder.DefaultJsonSerializerSettings, logDbModelEntityBuilder.JsonSerializerSettings);
        }

        #endregion

        #region Tests for BuildDbEntity method

        [Test]
        public void BuildDbEntity_When_Called_Creates_And_Returns_LogDbModel()
        {
            // Arrange
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();
            LogModel logData = Samples.LogModels.WithTagsAndItems();

            // Act
            Object logDbModel = logDbModelEntityBuilder.BuildDbEntity(logData);

            // Assert
            Assert.IsInstanceOf<LogDbModel>(logDbModel);
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel((LogDbModel)logDbModel, logData);
        }
        [Test]
        public void BuildDbEntity_When_Called_Serializes_Log_Items_And_Tags_Using_JsonSerializerFacade()
        {
            // Arrange
            LogModel logData = Samples.LogModels.WithTagsAndItems();
            var jsonSerializerFacadeMock = new Mock<IJsonSerializerFacade>();
            jsonSerializerFacadeMock.Setup(x => x.Serialize(It.IsAny<string[]>(), It.IsAny<JsonSerializerSettings>())).Returns("serialized tags");
            jsonSerializerFacadeMock.Setup(x => x.Serialize(It.IsAny<LogItem[]>(), It.IsAny<JsonSerializerSettings>())).Returns("serialized log items");
            var jsonSerializerSettings = new JsonSerializerSettings();
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder(jsonSerializerSettings);
            logDbModelEntityBuilder.ResetJsonSerializerFacade(jsonSerializerFacadeMock.Object);

            // Act
            LogDbModel logDbModel = (LogDbModel)logDbModelEntityBuilder.BuildDbEntity(logData);

            // Assert
            jsonSerializerFacadeMock.Verify(x => x.Serialize(logData.Tags, jsonSerializerSettings), Times.Once);
            jsonSerializerFacadeMock.Verify(x => x.Serialize(logData.LogItems, jsonSerializerSettings), Times.Once);
            Assert.AreEqual("serialized tags", logDbModel.TagsJson);
            Assert.AreEqual("serialized log items", logDbModel.LogItemsJson);
        }
        [Test]
        public void BuildDbEntity_When_Called_Given_That_Tags_And_LogItems_Are_Null()
        {
            // Arrange
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();
            LogModel logData = Samples.LogModels.Standard();
            logData.Tags = null;
            logData.LogItems = null;

            // Act
            Object dbModelObj = logDbModelEntityBuilder.BuildDbEntity(logData);

            // Assert 
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel((LogDbModel)dbModelObj, logData);
        }
        [Test]
        public void BuildDbEntity_When_Called_Given_That_Log_Item_Contains_Self_Reference_Loop()
        {
            // Arrange
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();
            LogModel logData = Samples.LogModels.WithTagsAndItems();
            // Object containing self reference loop.
            logData.LogItems[0].Value = new ClassWithCircularReference();

            // Act
            LogDbModel logDbModel = (LogDbModel)logDbModelEntityBuilder.BuildDbEntity(logData);

            // Assert  
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            Assert.AreEqual(JsonConvert.SerializeObject(logData.LogItems, jsonSerializerSettings), logDbModel.LogItemsJson);
        }

        #endregion

        #region Tests for ResetJsonSerializerFacade

        [Test]
        public void ResetJsonSerializerFacade_When_Called_Replaces_JsonSerializerFacade()
        {
            // Arrange
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Act
            logDbModelEntityBuilder.ResetJsonSerializerFacade(jsonSerializerFacade);

            // Assert
            Assert.AreEqual(jsonSerializerFacade, logDbModelEntityBuilder.JsonSerializerFacade);
        }
        [Test]
        public void ResetJsonSerializerFacade_When_Called_With_Null_JsonSerializerFacade_Throws_ArgumentNullException()
        {
            // Arrange
            LogDbModelEntityBuilder logDbModelEntityBuilder = new LogDbModelEntityBuilder();
            var jsonSerializerFacade = new Mock<IJsonSerializerFacade>().Object;

            // Assert
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                logDbModelEntityBuilder.ResetJsonSerializerFacade(null as IJsonSerializerFacade);
            });
            Assert.AreEqual("jsonSerializerFacade", ex.ParamName);
        }

        #endregion
    }
}