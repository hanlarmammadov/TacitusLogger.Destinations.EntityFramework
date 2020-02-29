using Microsoft.EntityFrameworkCore;
using System; 

namespace TacitusLogger.Destinations.EntityFramework
{
    public class FactoryMethodDbContextProvider : IDbContextProvider
    {
        private readonly LogModelFunc<DbContext> _factoryMethod;

        public FactoryMethodDbContextProvider(LogModelFunc<DbContext> factoryMethod)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException("factoryMethod");
        }

        public LogModelFunc<DbContext> FactoryMethod => _factoryMethod;

        public DbContext GetDbContext(LogModel logData)
        {
            return _factoryMethod(logData);
        }
    }
}
