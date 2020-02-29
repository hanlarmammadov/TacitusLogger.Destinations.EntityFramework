using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.EntityFramework
{
    public class EntityFrameworkDestination : ILogDestination
    {
        private readonly IDbContextProvider _dbContextProvider;
        private readonly IDbEntityBuilder _dbEntityBuilder;

        public EntityFrameworkDestination(IDbContextProvider dbContextProvider, IDbEntityBuilder dbEntityBuilder)
        {
            _dbContextProvider = dbContextProvider ?? throw new ArgumentNullException("dbContextProvider");
            _dbEntityBuilder = dbEntityBuilder ?? throw new ArgumentNullException("dbEntityBuilder");
        }

        public IDbContextProvider ContextProvider => _dbContextProvider;
        public IDbEntityBuilder EntityBuilder => _dbEntityBuilder;

        public void Send(LogModel[] logs)
        {
            if (logs.Length == 1)
            {
                // Get DbContext that will be used as a UoW.
                DbContext dbContext = _dbContextProvider.GetDbContext(logs[0]) ?? throw new Exception("Context provider returned null DB context");
                // Get entity that will persisted. 
                Object entity = _dbEntityBuilder.BuildDbEntity(logs[0]) ?? throw new Exception("Entity builder returned null entity");
                // Persist the entity.
                dbContext.Add(entity);
                dbContext.SaveChanges();
            }
            else
            {
                var contexts = new DbContext[logs.Length];
                for (int i = 0; i < logs.Length; i++)
                {
                    DbContext dbContext = _dbContextProvider.GetDbContext(logs[i]) ?? throw new Exception("Context provider returned null DbContext");
                    dbContext.Add(_dbEntityBuilder.BuildDbEntity(logs[i]) ?? throw new Exception("Entity builder returned null entity"));
                    contexts[i] = dbContext;
                }
                for (int i = 0; i < contexts.Length; i++)
                    contexts[i].SaveChanges();
            }
        }
        public async Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);

            if (logs.Length == 1)
            {
                // Get DbContext that will be used as a UoW.
                DbContext dbContext = _dbContextProvider.GetDbContext(logs[0]) ?? throw new Exception("Context provider returned null DbContext");
                // Get entity that will persisted. 
                Object entity = _dbEntityBuilder.BuildDbEntity(logs[0]) ?? throw new Exception("Entity builder returned null entity");
                // Persist the entity.
                await dbContext.AddAsync(entity);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var contexts = new DbContext[logs.Length];
                for (int i = 0; i < logs.Length; i++)
                {
                    DbContext dbContext = _dbContextProvider.GetDbContext(logs[i]) ?? throw new Exception("Context provider returned null DbContext");
                    await dbContext.AddAsync(_dbEntityBuilder.BuildDbEntity(logs[i]) ?? throw new Exception("Entity builder returned null entity"), cancellationToken);
                    contexts[i] = dbContext;
                }
                for (int i = 0; i < contexts.Length; i++)
                    await contexts[i].SaveChangesAsync(cancellationToken);
            }
        }
        public void Dispose()
        {

        }
    }
}
