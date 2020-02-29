using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework.IntegrationTests
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void LoggerBuilder_That_Creates_Logger_Containing_One_Log_Group_With_One_Entity_Framework_Destination_In_It()
        {
            var dbContextProvider = new Mock<IDbContextProvider>().Object;
            var dbEntityBuilder = new Mock<IDbEntityBuilder>().Object;

            var logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                               .EntityFramework()
                                                                               .WithDbContext(dbContextProvider)
                                                                               .WithDbEntity(dbEntityBuilder)
                                                                               .Add()
                                                                               .BuildLogger();
            // Assert
            // That the logger contains one log group
            var logGroup = logger.GetLogGroup("group1");
            Assert.AreEqual(1, logger.LogGroups.Count());
            // with destinations collection that contains one log destination
            var logDestinations = ((LogGroup)logger.GetLogGroup("group1")).LogDestinations;
            Assert.AreEqual(1, logDestinations.Count);
            // that is an instance of the EntityFrameworkDestination
            var entityFrameworkDestination = (EntityFrameworkDestination)logDestinations.Single();
            Assert.IsInstanceOf<EntityFrameworkDestination>(entityFrameworkDestination);
            // with db context provider
            Assert.AreEqual(dbContextProvider, entityFrameworkDestination.ContextProvider);
            // and db entity builder
            Assert.AreEqual(dbEntityBuilder, entityFrameworkDestination.EntityBuilder);
        }

        [Test]
        public void LoggerBuilder_That_Creates_Logger_Containing_One_Log_Group_With_One_Entity_Framework_Destination_With_Default_DbEntityBuilder()
        {
            var logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                               .EntityFramework()
                                                                               .WithDbContext(new Mock<IDbContextProvider>().Object)
                                                                               .Add()
                                                                               .BuildLogger();
            // Assert
            // That the logger contains one log group
            var logGroup = logger.GetLogGroup("group1");
            Assert.AreEqual(1, logger.LogGroups.Count());
            // with destinations collection that contains one log destination
            var logDestinations = ((LogGroup)logger.GetLogGroup("group1")).LogDestinations;
            Assert.AreEqual(1, logDestinations.Count);
            // that is an instance of the EntityFrameworkDestination
            var entityFrameworkDestination = (EntityFrameworkDestination)logDestinations.Single();
            Assert.IsInstanceOf<EntityFrameworkDestination>(entityFrameworkDestination);
            // with the default db entity builder
            Assert.IsInstanceOf<LogDbModelEntityBuilder>(entityFrameworkDestination.EntityBuilder);
        }

        [Test]
        public void LoggerBuilder_With_Entity_Framework_Destination_When_Db_Context_Provider_Was_Not_Specified_Throws_InvalidOperationException()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                var logger = (Logger)LoggerBuilder.Logger().NewLogGroup("group1").ForAllLogs()
                                                                                   .EntityFramework()
                                                                                   .WithDbEntity(new Mock<IDbEntityBuilder>().Object)
                                                                                   .Add()
                                                                                   .BuildLogger();
            });
        }
    }
}
