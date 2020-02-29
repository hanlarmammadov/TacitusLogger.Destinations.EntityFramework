using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework
{
    /// <summary>
    /// Adds Tacitus logger Entity Framework destination builder extension method to <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c> interface and its implementations.
    /// </summary>
    public static class LogGroupDestinationsBuilderEntityFrameworkExtensions
    {
        /// <summary>
        /// Initiate the adding a Entity Framework destination to the log group builder.
        /// </summary> 
        /// <returns>Tacitus logger Entity Framework destination builder.</returns>
        public static IEntityFrameworkDestinationBuilder EntityFramework(this ILogGroupDestinationsBuilder obj)
        {
            return new EntityFrameworkDestinationBuilder(obj);
        }
    }
}
