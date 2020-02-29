using Moq;
using NUnit.Framework;
using System;
using TacitusLogger.Builders;
using TacitusLogger.Destinations;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests.BuilderTests
{
    [TestFixture]
    public class EntityFrameworkDestinationBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_When_Called_Sets_LogGroupDestinationsBuilder()
        {
            // Arrange
            var logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(logGroupDestinationsBuilder);

            // Arrange
            Assert.AreEqual(logGroupDestinationsBuilder, entityFrameworkDestinationBuilder.LogGroupDestinationsBuilder);
        }

        [Test]
        public void Ctor_When_Called_Sets_Dependencies_To_Null()
        {
            // Act
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            // Arrange
            Assert.IsNull(entityFrameworkDestinationBuilder.DbContextProvider);
            Assert.IsNull(entityFrameworkDestinationBuilder.DbEntityBuilder);
        }

        #endregion

        #region Tests for WithDbContext method

        [Test]
        public void WithDbContext_When_Called_Sets_DbContextProvider()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var dbContextProvider = new Mock<IDbContextProvider>().Object;

            // Act
            entityFrameworkDestinationBuilder.WithDbContext(dbContextProvider);

            // Assert
            Assert.AreEqual(dbContextProvider, entityFrameworkDestinationBuilder.DbContextProvider);
        }

        [Test]
        public void WithDbContext_When_Called_With_Null_DbContextProvider_Throws_ArgumentNullException()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                entityFrameworkDestinationBuilder.WithDbContext(null as IDbContextProvider);
            });
        }

        [Test]
        public void WithDbContext_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            entityFrameworkDestinationBuilder.WithDbContext(new Mock<IDbContextProvider>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                entityFrameworkDestinationBuilder.WithDbContext(new Mock<IDbContextProvider>().Object);
            });
        }

        #endregion

        #region Tests for WithDbEntity method

        [Test]
        public void WithDbEntity_When_Called_Sets_DbEntityBuilder()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            var dbEntityBuilder = new Mock<IDbEntityBuilder>().Object;

            // Act
            entityFrameworkDestinationBuilder.WithDbEntity(dbEntityBuilder);

            // Assert
            Assert.AreEqual(dbEntityBuilder, entityFrameworkDestinationBuilder.DbEntityBuilder);
        }

        [Test]
        public void WithDbEntity_When_Called_With_Null_DbEntityBuilder_Throws_ArgumentNullException()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);

            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                entityFrameworkDestinationBuilder.WithDbEntity(null as IDbEntityBuilder);
            });
        }

        [Test]
        public void WithDbEntity_When_Called_Given_That_Already_Set_Throws_InvalidOperationException()
        {
            // Arrange
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            entityFrameworkDestinationBuilder.WithDbEntity(new Mock<IDbEntityBuilder>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                entityFrameworkDestinationBuilder.WithDbEntity(new Mock<IDbEntityBuilder>().Object);
            });
        }

        #endregion

        #region Tests for Add method

        [Test]
        public void Add_When_Called_Creates_New_Destination_Sends_It_To_CustomDestination_Method_And_Returns_Its_Result()
        {
            // Arrange
            // Creating the destination builder.
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            logGroupDestinationsBuilderMock.Setup(x => x.CustomDestination(It.IsAny<ILogDestination>())).Returns(logGroupDestinationsBuilderMock.Object);
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            // Adding context provider.
            var dbContextProvider = new Mock<IDbContextProvider>().Object;
            entityFrameworkDestinationBuilder.WithDbContext(dbContextProvider);
            // Adding entity builder.
            var dbEntityBuilder = new Mock<IDbEntityBuilder>().Object;
            entityFrameworkDestinationBuilder.WithDbEntity(dbEntityBuilder);

            // Act
            var returned = entityFrameworkDestinationBuilder.Add();

            // Arrange
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EntityFrameworkDestination>(d => d.ContextProvider == dbContextProvider &&
                                                                                                                   d.EntityBuilder == dbEntityBuilder)));
            Assert.AreEqual(logGroupDestinationsBuilderMock.Object, returned);
        }

        [Test]
        public void Add_When_Called_Given_That_DbContextProvider_Was_Not_Set_During_The_Build_Throws_InvalidOperationException()
        {
            // Arrange
            // Creating the destination builder. 
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(new Mock<ILogGroupDestinationsBuilder>().Object);
            // Adding entity builder. 
            entityFrameworkDestinationBuilder.WithDbEntity(new Mock<IDbEntityBuilder>().Object);

            Assert.Catch<InvalidOperationException>(() =>
            {
                // Act
                entityFrameworkDestinationBuilder.Add();
            });
        }

        [Test]
        public void Add_When_Called_Given_That_DbEntityBuilder_Was_Not_Set_During_The_Build_Throws_InvalidOperationException()
        {
            // Arrange
            // Creating the destination builder.
            var logGroupDestinationsBuilderMock = new Mock<ILogGroupDestinationsBuilder>();
            logGroupDestinationsBuilderMock.Setup(x => x.CustomDestination(It.IsAny<ILogDestination>())).Returns(logGroupDestinationsBuilderMock.Object);
            EntityFrameworkDestinationBuilder entityFrameworkDestinationBuilder = new EntityFrameworkDestinationBuilder(logGroupDestinationsBuilderMock.Object);
            // Adding context provider.
            var dbContextProvider = new Mock<IDbContextProvider>().Object;
            entityFrameworkDestinationBuilder.WithDbContext(dbContextProvider); 

            // Act
            entityFrameworkDestinationBuilder.Add();

            // Arrange
            logGroupDestinationsBuilderMock.Verify(x => x.CustomDestination(It.Is<EntityFrameworkDestination>(d => d.ContextProvider == dbContextProvider &&
                                                                                                                   d.EntityBuilder is LogDbModelEntityBuilder))); 
        }

        #endregion
    }
}
