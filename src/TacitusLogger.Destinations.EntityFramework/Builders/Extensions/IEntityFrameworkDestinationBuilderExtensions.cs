using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace TacitusLogger.Destinations.EntityFramework
{
    public static class IEntityFrameworkDestinationBuilderExtensions
    {
        #region Extension methods related to WithDbContext method

        public static IEntityFrameworkDestinationBuilder WithDbContext(this IEntityFrameworkDestinationBuilder self, DbContext dbContext)
        {
            return self.WithDbContext(new DbContextProvider(dbContext));
        }
        public static IEntityFrameworkDestinationBuilder WithDbContext(this IEntityFrameworkDestinationBuilder self, LogModelFunc<DbContext> factoryMethod)
        {
            return self.WithDbContext(new FactoryMethodDbContextProvider(factoryMethod));
        }

        #endregion

        #region Extension methods related to WithDbEntity method

        public static IEntityFrameworkDestinationBuilder WithDbEntity(this IEntityFrameworkDestinationBuilder self, LogModelFunc<Object> factoryMethod)
        {
            return self.WithDbEntity(new FactoryMethodDbEntityBuilder(factoryMethod));
        }
        public static IEntityFrameworkDestinationBuilder WithLogModelDbEntity(this IEntityFrameworkDestinationBuilder self, JsonSerializerSettings jsonSerializerSettings)
        {
            return self.WithDbEntity(new LogDbModelEntityBuilder(jsonSerializerSettings));
        }
        public static IEntityFrameworkDestinationBuilder WithLogModelDbEntity(this IEntityFrameworkDestinationBuilder self)
        {
            return self.WithDbEntity(new LogDbModelEntityBuilder());
        }

        #endregion
    }
}
