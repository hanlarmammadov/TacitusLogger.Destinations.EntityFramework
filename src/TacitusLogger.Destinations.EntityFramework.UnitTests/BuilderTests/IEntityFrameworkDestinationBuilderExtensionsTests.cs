using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests.BuilderTests
{
    [TestFixture]
    public class IEntityFrameworkDestinationBuilderExtensionsTests
    {
        #region Tests for WithDbContext method overloads

        [Test]
        public void WithDbContext_Taking_DbContext_When_Called_Calls_WithDbContext_Method_Of_The_Builder()
        {
            // Arrange
            var entityFrameworkDestinationBuilder = new Mock<IEntityFrameworkDestinationBuilder>();
            entityFrameworkDestinationBuilder.Setup(x => x.WithDbContext(It.IsAny<IDbContextProvider>())).Returns(entityFrameworkDestinationBuilder.Object);
            DbContext dbContext = new Mock<DbContext>().Object;

            // Act
            var returned = IEntityFrameworkDestinationBuilderExtensions.WithDbContext(entityFrameworkDestinationBuilder.Object, dbContext);

            // Assert
            entityFrameworkDestinationBuilder.Verify(x => x.WithDbContext(It.Is<DbContextProvider>(c => c.DbContext == dbContext)), Times.Once);
            Assert.AreEqual(entityFrameworkDestinationBuilder.Object, returned);

        }

        [Test]
        public void WithDbContext_Taking_FactoryMethod_When_Called_Calls_WithDbContext_Method_Of_The_Builder()
        {
            // Arrange
            var entityFrameworkDestinationBuilder = new Mock<IEntityFrameworkDestinationBuilder>();
            entityFrameworkDestinationBuilder.Setup(x => x.WithDbContext(It.IsAny<IDbContextProvider>())).Returns(entityFrameworkDestinationBuilder.Object);
            LogModelFunc<DbContext> factoryMethod = d => null;

            // Act
            var returned = IEntityFrameworkDestinationBuilderExtensions.WithDbContext(entityFrameworkDestinationBuilder.Object, factoryMethod);

            // Assert
            entityFrameworkDestinationBuilder.Verify(x => x.WithDbContext(It.Is<FactoryMethodDbContextProvider>(c => c.FactoryMethod == factoryMethod)), Times.Once);
            Assert.AreEqual(entityFrameworkDestinationBuilder.Object, returned);

        }

        #endregion

        #region Tests for WithDbEntity method 

        [Test]
        public void WithDbEntity_Taking_FactoryMethod_When_Called_Calls_WithDbEntity_Method_Of_The_Builder()
        {
            // Arrange
            var entityFrameworkDestinationBuilder = new Mock<IEntityFrameworkDestinationBuilder>();
            entityFrameworkDestinationBuilder.Setup(x => x.WithDbEntity(It.IsAny<IDbEntityBuilder>())).Returns(entityFrameworkDestinationBuilder.Object);
            LogModelFunc<Object> factoryMethod = d => null;

            // Act
            var returned = IEntityFrameworkDestinationBuilderExtensions.WithDbEntity(entityFrameworkDestinationBuilder.Object, factoryMethod);

            // Assert
            entityFrameworkDestinationBuilder.Verify(x => x.WithDbEntity(It.Is<FactoryMethodDbEntityBuilder>(c => c.FactoryMethod == factoryMethod)), Times.Once);
            Assert.AreEqual(entityFrameworkDestinationBuilder.Object, returned);
        }

        #endregion

        #region Tests for WithLogModelDbEntity method overloads

        [Test]
        public void WithLogModelDbEntity_Taking_JsonSerializerSettings_When_Called_Calls_WithDbEntity_Method_Of_The_Builder()
        {
            // Arrange
            var entityFrameworkDestinationBuilder = new Mock<IEntityFrameworkDestinationBuilder>();
            entityFrameworkDestinationBuilder.Setup(x => x.WithDbEntity(It.IsAny<IDbEntityBuilder>())).Returns(entityFrameworkDestinationBuilder.Object);
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();

            // Act
            var returned = IEntityFrameworkDestinationBuilderExtensions.WithLogModelDbEntity(entityFrameworkDestinationBuilder.Object, jsonSerializerSettings);

            // Assert
            entityFrameworkDestinationBuilder.Verify(x => x.WithDbEntity(It.Is<LogDbModelEntityBuilder>(c => c.JsonSerializerSettings == jsonSerializerSettings)), Times.Once);
            Assert.AreEqual(entityFrameworkDestinationBuilder.Object, returned);
        }

        [Test]
        public void WithLogModelDbEntity_Taking_No_Params_When_Called_Calls_WithDbEntity_Method_Of_The_Builder()
        {
            // Arrange
            var entityFrameworkDestinationBuilder = new Mock<IEntityFrameworkDestinationBuilder>();
            entityFrameworkDestinationBuilder.Setup(x => x.WithDbEntity(It.IsAny<IDbEntityBuilder>())).Returns(entityFrameworkDestinationBuilder.Object);

            // Act
            var returned = IEntityFrameworkDestinationBuilderExtensions.WithLogModelDbEntity(entityFrameworkDestinationBuilder.Object);

            // Assert
            entityFrameworkDestinationBuilder.Verify(x => x.WithDbEntity(It.Is<LogDbModelEntityBuilder>(c => c.JsonSerializerSettings == LogDbModelEntityBuilder.DefaultJsonSerializerSettings)), Times.Once);
            Assert.AreEqual(entityFrameworkDestinationBuilder.Object, returned);
        }

        #endregion
    }
}
