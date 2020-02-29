using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework
{
    public interface IEntityFrameworkDestinationBuilder : IDestinationBuilder
    {
        IEntityFrameworkDestinationBuilder WithDbContext(IDbContextProvider dbContextProvider);
        IEntityFrameworkDestinationBuilder WithDbEntity(IDbEntityBuilder dbEntityBuilder);
    }
}
