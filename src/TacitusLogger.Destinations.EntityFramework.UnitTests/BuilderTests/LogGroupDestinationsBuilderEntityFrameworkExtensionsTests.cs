using Moq;
using NUnit.Framework;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests.BuilderTests
{
    [TestFixture]
    public class LogGroupDestinationsBuilderEntityFrameworkExtensionsTests
    {
        [Test]
        public void EntityFramework_When_Called_Creates_And_Returns_EntityFrameworkDestinationBuilder()
        {
            // Arrange
            ILogGroupDestinationsBuilder logGroupDestinationsBuilder = new Mock<ILogGroupDestinationsBuilder>().Object;

            // Act
            IEntityFrameworkDestinationBuilder destinationBuilderReturned = LogGroupDestinationsBuilderEntityFrameworkExtensions.EntityFramework(logGroupDestinationsBuilder);

            // Assert
            Assert.IsInstanceOf<EntityFrameworkDestinationBuilder>(destinationBuilderReturned);
            Assert.AreEqual(logGroupDestinationsBuilder, ((EntityFrameworkDestinationBuilder)destinationBuilderReturned).LogGroupDestinationsBuilder);
        }
    }
}
