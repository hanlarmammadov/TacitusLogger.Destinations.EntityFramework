using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests
{
    [TestFixture]
    public class FactoryMethodDbContextProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Factory_Method_When_Called_Sets_Factory_Method()
        {
            // Arrange
            LogModelFunc<DbContext> factoryMethod = d => null;

            // Act
            FactoryMethodDbContextProvider factoryMethodDbContextProvider = new FactoryMethodDbContextProvider(factoryMethod);

            // Assert
            Assert.AreEqual(factoryMethod, factoryMethodDbContextProvider.FactoryMethod);
        } 
        [Test]
        public void Ctor_Taking_Factory_Method_When_Called_With_Null_Factory_Method_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FactoryMethodDbContextProvider factoryMethodDbContextProvider = new FactoryMethodDbContextProvider(null as LogModelFunc<DbContext>);
            });
        }

        #endregion

        #region Tests for GetDbContext method

        [Test]
        public void GetDbContext_Taking_LogModel_When_Called_Returns_Result_From_Factory_Method()
        {
            // Arrange
            DbContext contextReturnedFromFactoryMethod = new Mock<DbContext>().Object;
            LogModelFunc<DbContext> factoryMethod = d => contextReturnedFromFactoryMethod;
            FactoryMethodDbContextProvider factoryMethodDbContextProvider = new FactoryMethodDbContextProvider(factoryMethod);

            // Act
            var contextFromMethod = factoryMethodDbContextProvider.GetDbContext(new LogModel());

            // Assert
            Assert.AreEqual(contextReturnedFromFactoryMethod, contextFromMethod);
        } 
        [Test]
        public void GetDbContext_Taking_LogModel_When_Called_Several_Times_Returns_Result_From_Factory_Method()
        {
            // Arrange
            DbContext contextReturnedFromFactoryMethod = new Mock<DbContext>().Object;
            LogModelFunc<DbContext> factoryMethod = d => contextReturnedFromFactoryMethod;
            FactoryMethodDbContextProvider factoryMethodDbContextProvider = new FactoryMethodDbContextProvider(factoryMethod);

            // Act
            var contextFromMethod1 = factoryMethodDbContextProvider.GetDbContext(new LogModel());
            var contextFromMethod2 = factoryMethodDbContextProvider.GetDbContext(new LogModel());

            // Assert
            Assert.AreEqual(contextReturnedFromFactoryMethod, contextFromMethod1);
            Assert.AreEqual(contextReturnedFromFactoryMethod, contextFromMethod2);
        }

        #endregion 
    }
}
