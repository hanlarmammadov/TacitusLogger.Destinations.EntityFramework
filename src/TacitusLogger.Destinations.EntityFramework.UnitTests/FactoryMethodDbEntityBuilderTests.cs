using NUnit.Framework;
using System; 

namespace TacitusLogger.Destinations.EntityFramework.UnitTests
{
    [TestFixture]
    public class FactoryMethodDbEntityBuilderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_Factory_Method_When_Called_Sets_Factory_Method()
        {
            // Arrange
            LogModelFunc<Object> factoryMethod = d => null;

            // Act
            FactoryMethodDbEntityBuilder factoryMethodDbEntityBuilder = new FactoryMethodDbEntityBuilder(factoryMethod);

            // Assert
            Assert.AreEqual(factoryMethod, factoryMethodDbEntityBuilder.FactoryMethod);
        }

        [Test]
        public void Ctor_Taking_Factory_Method_When_Called_With_Null_Factory_Method_Throws_ArgumentNullException()
        {
            Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                FactoryMethodDbEntityBuilder factoryMethodDbEntityBuilder = new FactoryMethodDbEntityBuilder(null as LogModelFunc<Object>);
            });
        }

        #endregion

        #region Tests for BuildDbEntity method

        [Test]
        public void BuildDbEntity_When_Called_Returns_Result_Of_Factory_Method()
        {
            // Arrange
            Object resultFromFactoryMethod = new Object();
            LogModelFunc<Object> factoryMethod = d => resultFromFactoryMethod;
            FactoryMethodDbEntityBuilder factoryMethodDbEntityBuilder = new FactoryMethodDbEntityBuilder(factoryMethod);

            // Act
            var resultFromMethod = factoryMethodDbEntityBuilder.BuildDbEntity(new LogModel());

            // Assert
            Assert.AreEqual(resultFromFactoryMethod, resultFromMethod);
        }

        #endregion
    }
}
