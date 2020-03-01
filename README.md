# TacitusLogger.Destinations.EntityFramework

> Extension destination for TacitusLogger that sends logs to database using Entity Framework ORM.
 
Dependencies:  
* NET Standard >= 2.0  
* TacitusLogger >= 0.3.0  
* Microsoft.EntityFrameworkCore >= 2.0.0  
  
> Attention: `TacitusLogger.Destinations.EntityFramework` is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation

The NuGet <a href="http://example.com/" target="_blank">package</a>:

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