using Microsoft.EntityFrameworkCore;
using Moq;

namespace TacitusLogger.Destinations.EntityFramework.Examples
{
    class Configuring
    {
        private DbContext dbContext;
        private DbContext dbContext1;
        private DbContext dbContext2;

        public void Adding_Entity_Framework_Destination_With_Minimal_Configuration()
        {
            IDbContextProvider dbContextProvider = new DbContextProvider(dbContext);
            IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, dbEntityBuilder);

            Logger logger = new Logger();
            logger.AddLogDestinations(entityFrameworkDestination);
        }
        public void Adding_Entity_Framework_Destination_With_DbContext_Function()
        {
            IDbContextProvider factoryMethodDbContextProvider = new FactoryMethodDbContextProvider((logModel) =>
            {
                if (logModel.HasTag("App1"))
                    return dbContext1;
                else
                    return dbContext2;
            });
            IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(factoryMethodDbContextProvider, dbEntityBuilder);

            Logger logger = new Logger();
            logger.AddLogDestinations(entityFrameworkDestination);
        }
        public void Adding_Entity_Framework_Destination_With_Custom_DbContext_Provider()
        {
            IDbContextProvider customDbContextProvider = new Mock<IDbContextProvider>().Object;
            IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(customDbContextProvider, dbEntityBuilder);

            Logger logger = new Logger();
            logger.AddLogDestinations(entityFrameworkDestination);
        }
        public void Adding_Entity_Framework_Destination_With_DbEntity_Factory_Method()
        {
            IDbContextProvider dbContextProvider = new DbContextProvider(dbContext);
            IDbEntityBuilder factoryMethodDbEntityBuilder = new FactoryMethodDbEntityBuilder((logModel) =>
            {
                return new MyCustomDbEntity()
                {
                    LogId = logModel.LogId,
                    Context = logModel.Context,
                    Description = logModel.Description,
                    LogType = logModel.LogType,
                    LogDate = logModel.LogDate
                };
            });
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, factoryMethodDbEntityBuilder);

            Logger logger = new Logger();
            logger.AddLogDestinations(entityFrameworkDestination);
        }
        public void Adding_Entity_Framework_Destination_With_Custom_DbEntity_Builder()
        {
            IDbContextProvider dbContextProvider = new DbContextProvider(dbContext);
            IDbEntityBuilder customDbEntityBuilder = new Mock<IDbEntityBuilder>().Object;
            EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, customDbEntityBuilder);

            Logger logger = new Logger();
            logger.AddLogDestinations(entityFrameworkDestination);
        }
    }
}
