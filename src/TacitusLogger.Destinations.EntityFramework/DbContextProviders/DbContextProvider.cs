using Microsoft.EntityFrameworkCore;
using System;

namespace TacitusLogger.Destinations.EntityFramework
{
    public class DbContextProvider : IDbContextProvider
    {
        private readonly DbContext _dbContext;

        public DbContextProvider(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
        }

        internal DbContext DbContext => _dbContext;

        public DbContext GetDbContext(LogModel logData)
        {
            return _dbContext;
        }
    }
}
