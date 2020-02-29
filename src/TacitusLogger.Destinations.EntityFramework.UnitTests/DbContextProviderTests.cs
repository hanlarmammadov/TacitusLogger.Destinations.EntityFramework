using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Destinations.EntityFramework.UnitTests
{
    [TestFixture]
    public class DbContextProviderTests
    {
        #region Ctor tests

        [Test]
        public void Ctor_Taking_DbContext_When_Called_Sets_DbContext()
        {
            // Arrange
            DbContext dbContext = new Mock<DbContext>().Object;

            // Act
            DbContextProvider dbContextProvider = new DbContextProvider(dbContext);

            // Arrange
            Assert.AreEqual(dbContext, dbContextProvider.DbContext);
        }
        [Test]
        public void Ctor_Taking_DbContext_When_Called_With_Null_DbContext_Throws_ArgumentNullException()
        {
            var ex = Assert.Catch<ArgumentNullException>(() =>
            {
                // Act
                DbContextProvider dbContextProvider = new DbContextProvider(null as DbContext);
            });
            Assert.AreEqual("dbContext", ex.ParamName);
        }

        #endregion

        #region Tests for GetDbContext method

        [Test]
        public void GetDbContext_When_Called_Returns_DbContext_That_Was_Set_During_Init()
        {
            // Arrange
            DbContext dbContext = new Mock<DbContext>().Object;
            DbContextProvider dbContextProvider = new DbContextProvider(dbContext);

            // Act
            var dbContextReturned = dbContextProvider.GetDbContext(new LogModel());

            // Arrange
            Assert.AreEqual(dbContext, dbContextReturned);
        }
        [Test]
        public void GetDbContext_When_Called_With_Null_LogModel_Returns_DbContext_That_Was_Set_During_Init()
        {
            // Arrange
            DbContext dbContext = new Mock<DbContext>().Object;
            DbContextProvider dbContextProvider = new DbContextProvider(dbContext);

            // Act
            var dbContextReturned = dbContextProvider.GetDbContext(null as LogModel);

            // Arrange
            Assert.AreEqual(dbContext, dbContextReturned);
        }
        [Test]
        public void GetDbContext_When_Called_Several_Times_Returns_DbContext_That_Was_Set_During_Init()
        {
            // Arrange
            DbContext dbContext = new Mock<DbContext>().Object;
            DbContextProvider dbContextProvider = new DbContextProvider(dbContext);

            // Act
            var dbContextReturned1 = dbContextProvider.GetDbContext(new LogModel());
            var dbContextReturned2 = dbContextProvider.GetDbContext(new LogModel());

            // Arrange
            Assert.AreEqual(dbContext, dbContextReturned1);
            Assert.AreEqual(dbContext, dbContextReturned2);
        }

        #endregion
    }
}
