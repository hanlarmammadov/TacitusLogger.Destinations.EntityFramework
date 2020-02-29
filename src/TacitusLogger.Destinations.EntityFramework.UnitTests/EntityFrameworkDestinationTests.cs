using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests
{
    [TestFixture]
    public class EntityFrameworkDestinationTests
    {
        #region Tests for Ctor

        [Test]
        public void Ctor_Taking_Context_Provider_And_EntityBuilder_When_Called_Sets_Dependencies()
        {
            // Arrange
            IDbContextProvider dbContextProvider = new Mock<IDbContextProvider>().Object;
            IDbEntityBuilder dbEntityBuilder = new Mock<IDbEntityBuilder>().Object;

            // Act
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, dbEntityBuilder);

            // Assert
            Assert.AreEqual(dbContextProvider, entityFrameworkDestination.ContextProvider);
            Assert.AreEqual(dbEntityBuilder, entityFrameworkDestination.EntityBuilder);
        }
        [Test]
        public void Ctor_Taking_Context_Provider_And_EntityBuilder_When_Called_With_Null_Context_Provider_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(null as IDbContextProvider, new Mock<IDbEntityBuilder>().Object);
            });
        }
        [Test]
        public void Ctor_Taking_Context_Provider_And_EntityBuilder_When_Called_With_Null_Entity_Builder_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(new Mock<IDbContextProvider>().Object, null as IDbEntityBuilder);
            });
        }

        #endregion

        #region Tests for Send method

        [Test]
        public void Send_Taking_Context_Provider_And_EntityBuilder_When_Called_With_One_Log()
        {
            // Arrange
            LogModel logData = new LogModel();
            // Context provider.
            var dbContextProviderMock = new Mock<IDbContextProvider>();
            var dbContextMock = new Mock<DbContext>();
            dbContextProviderMock.Setup(x => x.GetDbContext(logData)).Returns(dbContextMock.Object);
            // Entity builder.
            var dbEntityBuilderMock = new Mock<IDbEntityBuilder>();
            Object obj = new { };
            dbEntityBuilderMock.Setup(x => x.BuildDbEntity(logData)).Returns(obj);
            // Entity framework destination.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProviderMock.Object, dbEntityBuilderMock.Object);

            // Act
            entityFrameworkDestination.Send(new LogModel[] { logData });

            // Assert
            // Context object is requested from context provider
            dbContextProviderMock.Verify(x => x.GetDbContext(logData), Times.Once);
            // Db entity is requested from entity builder.
            dbEntityBuilderMock.Verify(x => x.BuildDbEntity(logData), Times.Once);
            // Db entity is added to context .
            dbContextMock.Verify(x => x.Add(obj), Times.Once);
            // Changes in context is saved.
            dbContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestCase(2)]
        [TestCase(4)]
        [TestCase(21)]
        public void Send_Taking_Context_Provider_And_EntityBuilder_When_Called_With_Several_Logs(int N)
        {
            // Arrange
            // logs array 
            Dictionary<LogModel, Object> dict = new Dictionary<LogModel, Object>();
            for (int i = 0; i < N; i++)
                dict.Add(new LogModel(), new Object());
            LogModel[] logs = dict.Keys.ToArray();
            Object[] objects = dict.Values.ToArray();
            // Context provider.
            var dbContextProviderMock = new Mock<IDbContextProvider>();
            var dbContextMock = new Mock<DbContext>();
            dbContextProviderMock.Setup(x => x.GetDbContext(It.IsIn(logs))).Returns(dbContextMock.Object);
            // Entity builder.
            var dbEntityBuilderMock = new Mock<IDbEntityBuilder>();
            dbEntityBuilderMock.Setup(x => x.BuildDbEntity(It.IsIn(logs))).Returns((LogModel d) => dict[d]);
            // Entity framework destination.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProviderMock.Object, dbEntityBuilderMock.Object);

            // Act
            entityFrameworkDestination.Send(logs);

            // Assert
            // Context object is requested from context provider
            dbContextProviderMock.Verify(x => x.GetDbContext(It.IsIn(logs)), Times.Exactly(N));
            // Db entity is requested from entity builder.
            dbEntityBuilderMock.Verify(x => x.BuildDbEntity(It.IsIn(logs)), Times.Exactly(N));
            // Db entity is added to context .
            dbContextMock.Verify(x => x.Add(It.IsIn<Object>(objects)), Times.Exactly(N));
            // Changes in context is saved.
            dbContextMock.Verify(x => x.SaveChanges(), Times.Exactly(N));

            for (int i = 0; i < N; i++)
            {
                // Context object is requested from context provider
                dbContextProviderMock.Verify(x => x.GetDbContext(logs[i]), Times.Exactly(1));
                // Db entity is requested from entity builder.
                dbEntityBuilderMock.Verify(x => x.BuildDbEntity(logs[i]), Times.Exactly(1));
            }
        }

        #endregion

        #region Tests for SendAsync method

        [Test]
        public async Task SendAsync_Taking_Context_Provider_And_EntityBuilder_When_Called_With_One_Log()
        {
            // Arrange
            LogModel logData = new LogModel();
            // Context provider.
            var dbContextProviderMock = new Mock<IDbContextProvider>();
            var dbContextMock = new Mock<DbContext>();
            dbContextProviderMock.Setup(x => x.GetDbContext(logData)).Returns(dbContextMock.Object);
            // Entity builder.
            var dbEntityBuilderMock = new Mock<IDbEntityBuilder>();
            Object obj = new { };
            dbEntityBuilderMock.Setup(x => x.BuildDbEntity(logData)).Returns(obj);
            // Entity framework destination.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProviderMock.Object, dbEntityBuilderMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await entityFrameworkDestination.SendAsync(new LogModel[] { logData }, cancellationToken);

            // Assert
            // Context object is requested from context provider.
            dbContextProviderMock.Verify(x => x.GetDbContext(logData), Times.Once);
            // Db entity is requested from entity builder.
            dbEntityBuilderMock.Verify(x => x.BuildDbEntity(logData), Times.Once);
            // Db entity is added to context.
            dbContextMock.Verify(x => x.AddAsync(obj, cancellationToken), Times.Once);
            // Changes in context is saved.
            dbContextMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once);
        }
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(21)]
        public async Task SendAsync_Taking_Context_Provider_And_EntityBuilder_When_Called_With_Several_Logs(int N)
        {
            // Arrange
            // logs array 
            Dictionary<LogModel, Object> dict = new Dictionary<LogModel, Object>();
            for (int i = 0; i < N; i++)
                dict.Add(new LogModel(), new Object());
            LogModel[] logs = dict.Keys.ToArray();
            Object[] objects = dict.Values.ToArray();
            // Context provider.
            var dbContextProviderMock = new Mock<IDbContextProvider>();
            var dbContextMock = new Mock<DbContext>();
            dbContextProviderMock.Setup(x => x.GetDbContext(It.IsIn(logs))).Returns(dbContextMock.Object);
            // Entity builder.
            var dbEntityBuilderMock = new Mock<IDbEntityBuilder>();
            dbEntityBuilderMock.Setup(x => x.BuildDbEntity(It.IsIn(logs))).Returns((LogModel d) => dict[d]);
            // Entity framework destination.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProviderMock.Object, dbEntityBuilderMock.Object);
            CancellationToken cancellationToken = new CancellationToken();

            // Act
            await entityFrameworkDestination.SendAsync(logs, cancellationToken);

            // Assert
            // Context object is requested from context provider
            dbContextProviderMock.Verify(x => x.GetDbContext(It.IsIn(logs)), Times.Exactly(N));
            // Db entity is requested from entity builder.
            dbEntityBuilderMock.Verify(x => x.BuildDbEntity(It.IsIn(logs)), Times.Exactly(N));
            // Db entity is added to context .
            dbContextMock.Verify(x => x.AddAsync(It.IsIn<Object>(objects), cancellationToken), Times.Exactly(N));
            // Changes in context is saved.
            dbContextMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Exactly(N));

            for (int i = 0; i < N; i++)
            {
                // Context object is requested from context provider
                dbContextProviderMock.Verify(x => x.GetDbContext(logs[i]), Times.Exactly(1));
                // Db entity is requested from entity builder.
                dbEntityBuilderMock.Verify(x => x.BuildDbEntity(logs[i]), Times.Exactly(1));
            }
        }
        [Test]
        public void SendAsync_Taking_Context_Provider_And_EntityBuilder_When_Called_With_Cancellation_Token()
        {
            // Arrange 
            // Context provider.
            var dbContextProviderMock = new Mock<IDbContextProvider>();
            // Entity builder.
            var dbEntityBuilderMock = new Mock<IDbEntityBuilder>();
            // Entity framework destination.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProviderMock.Object, dbEntityBuilderMock.Object);
            CancellationToken cancellationToken = new CancellationToken(canceled: true);

            // Assert 
            Assert.CatchAsync<OperationCanceledException>(async () =>
            {
                // Act
                await entityFrameworkDestination.SendAsync(new LogModel[] { new LogModel() }, cancellationToken);
            });
            dbContextProviderMock.Verify(x => x.GetDbContext(It.IsAny<LogModel>()), Times.Never);
            dbEntityBuilderMock.Verify(x => x.BuildDbEntity(It.IsAny<LogModel>()), Times.Never);
        }

        #endregion
    }
}
