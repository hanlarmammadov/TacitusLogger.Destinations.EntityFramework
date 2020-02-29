using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TacitusLogger.Destinations.EntityFramework.TestHelpers;
using TacitusLogger.Tests.Helpers;

namespace TacitusLogger.Destinations.EntityFramework.IntegrationTests
{
    [TestFixture]
    public class EntityFrameworkDestinationTests
    {
        [Test]
        public void Send_Taking_One_Log_Saves_Log_In_Db_Context()
        {

            var options = new DbContextOptionsBuilder<LogModelContext>()
                                      .UseInMemoryDatabase(databaseName: "Test1")
                                      .Options;
            var logDataContext = new LogModelContext(options);
            IDbContextProvider dbContextProvider = new DbContextProvider(logDataContext);
            IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, dbEntityBuilder);
            LogModel logData = Samples.LogModels.WithTagsAndItems();

            // Act
            entityFrameworkDestination.Send(new LogModel[] { logData });

            // Assert
            List<LogDbModel> logs = logDataContext.Logs.ToList();
            Assert.AreEqual(1, logs.Count);
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel(logs[0], logData);
        }
        [Test]
        public void Send_Taking_Several_Logs_Saves_Log_In_Db_Context()
        {
            var options = new DbContextOptionsBuilder<LogModelContext>()
                                      .UseInMemoryDatabase(databaseName: "Test2")
                                      .Options;
            var logDataContext = new LogModelContext(options);
            IDbContextProvider dbContextProvider = new DbContextProvider(logDataContext);
            IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, dbEntityBuilder);
            LogModel logData1 = Samples.LogModels.WithTagsAndItems(LogType.Warning);
            logData1.LogId = "LogId1";
            LogModel logData2 = Samples.LogModels.WithTagsAndItems(LogType.Error);
            logData2.LogId = "LogId2";
            LogModel logData3 = Samples.LogModels.WithTagsAndItems(LogType.Info);
            logData3.LogId = "LogId3";

            // Act
            entityFrameworkDestination.Send(new LogModel[] { logData1, logData2, logData3 });

            // Assert
            List<LogDbModel> logs = logDataContext.Logs.ToList();
            Assert.AreEqual(3, logs.Count);
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel(logs[0], logData1);
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel(logs[1], logData2);
            LogDbModelAsserts.AssertLogModelModelIsEqualToLogModel(logs[2], logData3);
        }
    }
}
