# TacitusLogger.Destinations.EntityFramework

> Extension destination for TacitusLogger that sends logs to database using Entity Framework ORM.
 
Dependencies:  
* NET Standard >= 2.0  
* TacitusLogger >= 0.3.0  
* Microsoft.EntityFrameworkCore >= 2.0.0  
  
> Attention: `TacitusLogger.Destinations.EntityFramework` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="https://www.nuget.org/packages/TacitusLogger.Destinations.EntityFramework" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger.Destinations.EntityFramework
```

## Examples

### Adding Entity Framework destination with minimal configuration
Using builders:
```cs
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .EntityFramework()
                              .WithDbContext(dbContext)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IDbContextProvider dbContextProvider = new DbContextProvider(dbContext);
IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, dbEntityBuilder);

Logger logger = new Logger();
logger.AddLogDestinations(entityFrameworkDestination);
```
---
### With DbContext function
Using builders:
```cs
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
```
Directly:
```cs
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
```
---
### With custom DbContext provider
Using builders:
```cs
IDbContextProvider customDbContextProvider = new Mock<IDbContextProvider>().Object;
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .EntityFramework()
                              .WithDbContext(customDbContextProvider)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IDbContextProvider customDbContextProvider = new Mock<IDbContextProvider>().Object;
IDbEntityBuilder dbEntityBuilder = new LogDbModelEntityBuilder();
EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(customDbContextProvider, dbEntityBuilder);

Logger logger = new Logger();
logger.AddLogDestinations(entityFrameworkDestination);
```
---
### With log model DbEntity builder
Using builders:
```cs
JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .EntityFramework()
                              .WithDbContext(dbContext)
                              .WithLogModelDbEntity(jsonSerializerSettings)
                              .Add()
                          .BuildLogger();
```
---
### With DbEntity factory method
Using builders:
```cs
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
```
Directly:
```cs
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
```
---
### With custom DbEntity builder
Using builders:
```cs
IDbEntityBuilder customDbEntityBuilder = new Mock<IDbEntityBuilder>().Object;
var logger = LoggerBuilder.Logger()
                          .ForAllLogs()
                          .EntityFramework()
                              .WithDbContext(dbContext)
                              .WithDbEntity(customDbEntityBuilder)
                              .Add()
                          .BuildLogger();
```
Directly:
```cs
IDbContextProvider dbContextProvider = new DbContextProvider(dbContext);
IDbEntityBuilder customDbEntityBuilder = new Mock<IDbEntityBuilder>().Object;
EntityFrameworkDestination entityFrameworkDestination = new EntityFrameworkDestination(dbContextProvider, customDbEntityBuilder);

Logger logger = new Logger();
logger.AddLogDestinations(entityFrameworkDestination);
```

## License

[APACHE LICENSE 2.0](https://www.apache.org/licenses/LICENSE-2.0)

## See also

TacitusLogger:  

- [TacitusLogger](https://github.com/khanlarmammadov/TacitusLogger) - A simple yet powerful .NET logging library.

Destinations:

- [TacitusLogger.Destinations.MongoDb](https://github.com/khanlarmammadov/TacitusLogger.Destinations.MongoDb) - Extension destination for TacitusLogger that sends logs to MongoDb database.
- [TacitusLogger.Destinations.RabbitMq](https://github.com/khanlarmammadov/TacitusLogger.Destinations.RabbitMq) - Extension destination for TacitusLogger that sends logs to the RabbitMQ exchanges.
- [TacitusLogger.Destinations.Email](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Email) - Extension destination for TacitusLogger that sends logs as emails using SMTP protocol.
- [TacitusLogger.Destinations.Trace](https://github.com/khanlarmammadov/TacitusLogger.Destinations.Trace) - Extension destination for TacitusLogger that sends logs to System.Diagnostics.Trace listeners.  
  
Dependency injection:
- [TacitusLogger.DI.Ninject](https://github.com/khanlarmammadov/TacitusLogger.DI.Ninject) - Extension for Ninject dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.Autofac](https://github.com/khanlarmammadov/TacitusLogger.DI.Autofac) - Extension for Autofac dependency injection container that helps to configure and add TacitusLogger as a singleton.
- [TacitusLogger.DI.MicrosoftDI](https://github.com/khanlarmammadov/TacitusLogger.DI.MicrosoftDI) - Extension for Microsoft dependency injection container that helps to configure and add TacitusLogger as a singleton.  

Log contributors:

- [TacitusLogger.Contributors.ThreadInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.ThreadInfo) - Extension contributor for TacitusLogger that generates additional info related to the thread on which the logger method was called.
- [TacitusLogger.Contributors.MachineInfo](https://github.com/khanlarmammadov/TacitusLogger.Contributors.MachineInfo) - Extension contributor for TacitusLogger that generates additional info related to the machine on which the log was produced.