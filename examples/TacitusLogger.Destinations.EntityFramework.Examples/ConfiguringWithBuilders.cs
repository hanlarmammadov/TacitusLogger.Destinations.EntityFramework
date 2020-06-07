using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using TacitusLogger.Builders;

namespace TacitusLogger.Destinations.EntityFramework.Examples
{
    class ConfiguringWithBuilders
    {
        private DbContext dbContext;
        private DbContext dbContext1;
        private DbContext dbContext2;

        public void Adding_Entity_Framework_Destination_With_Minimal_Configuration()
        {
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(dbContext)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Entity_Framework_Destination_With_DbContext_Function()
        {
            LogModelFunc<DbContext> dbContextFunction = (logModel) =>
            {
                if (logModel.HasTag("App1"))
                    return dbContext1;
                else
                    return dbContext2;
            };
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(dbContextFunction)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Entity_Framework_Destination_With_Custom_DbContext_Provider()
        {
            IDbContextProvider customDbContextProvider = new Mock<IDbContextProvider>().Object;
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(customDbContextProvider)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Entity_Framework_Destination_With_Log_Model_DbEntity_Builder()
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(dbContext)
                                          .WithLogModelDbEntity(jsonSerializerSettings)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Entity_Framework_Destination_With_DbEntity_Factory_Method()
        {
            LogModelFunc<Object> dbEntityFactoryMethod = (logModel) =>
            {
                return new MyCustomDbEntity()
                {
                    LogId = logModel.LogId,
                    Context = logModel.Context,
                    Description = logModel.Description,
                    LogType = logModel.LogType,
                    LogDate = logModel.LogDate
                };
            };

            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(dbContext)
                                          .WithDbEntity(dbEntityFactoryMethod)
                                          .Add()
                                      .BuildLogger();
        }
        public void Adding_Entity_Framework_Destination_With_Custom_DbEntity_Builder()
        {
            IDbEntityBuilder customDbEntityBuilder = new Mock<IDbEntityBuilder>().Object;
            var logger = LoggerBuilder.Logger()
                                      .ForAllLogs()
                                      .EntityFramework()
                                          .WithDbContext(dbContext)
                                          .WithDbEntity(customDbEntityBuilder)
                                          .Add()
                                      .BuildLogger();
        }
    }
}
