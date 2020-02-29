using System;
using System.Collections.Generic;
using System.Text;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.EntityFramework.EntityFrameworkDestinationBuilder</c> class to the <c>LogGroupDestinationsBuilder</c>.
    /// </summary>
    public class EntityFrameworkDestinationBuilder : IEntityFrameworkDestinationBuilder
    {
        private readonly ILogGroupDestinationsBuilder _logGroupDestinationsBuilder;
        private IDbContextProvider _dbContextProvider;
        private IDbEntityBuilder _dbEntityBuilder;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.EntityFramework.EntityFrameworkDestinationBuilder</c> using parent <c>ILogGroupDestinationsBuilder</c> instance.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder"></param>
        public EntityFrameworkDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
        {
            _logGroupDestinationsBuilder = logGroupDestinationsBuilder;
        }


        public ILogGroupDestinationsBuilder LogGroupDestinationsBuilder => _logGroupDestinationsBuilder;
        public IDbContextProvider DbContextProvider => _dbContextProvider;
        public IDbEntityBuilder DbEntityBuilder => _dbEntityBuilder;


        public IEntityFrameworkDestinationBuilder WithDbContext(IDbContextProvider dbContextProvider)
        {
            if (_dbContextProvider != null)
                throw new InvalidOperationException("DbContext provider has already been set during the build");
            if (dbContextProvider == null)
                throw new ArgumentNullException("dbContextProvider");
            _dbContextProvider = dbContextProvider;
            return this;
        }
        public IEntityFrameworkDestinationBuilder WithDbEntity(IDbEntityBuilder dbEntityBuilder)
        {
            if (_dbEntityBuilder != null)
                throw new InvalidOperationException("Db entity builder has already been set during the build");
            if (dbEntityBuilder == null)
                throw new ArgumentNullException("dbEntityBuilder");
            _dbEntityBuilder = dbEntityBuilder;
            return this;
        }
        public ILogGroupDestinationsBuilder Add()
        {
            if (_dbContextProvider == null)
                throw new InvalidOperationException("DbContext provider was not been provided during the build");
            if (_dbEntityBuilder == null)
                _dbEntityBuilder = new LogDbModelEntityBuilder();

            // Create the destionation.
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(_dbContextProvider, _dbEntityBuilder);

            // Add to log group and return it.
            return _logGroupDestinationsBuilder.CustomDestination(entityFrameworkDestination);
        }
    }
}
