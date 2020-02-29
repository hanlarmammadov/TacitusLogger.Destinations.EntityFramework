using System; 

namespace TacitusLogger.Destinations.EntityFramework
{
    public class FactoryMethodDbEntityBuilder : IDbEntityBuilder
    {
        private readonly LogModelFunc<Object> _factoryMethod;

        public FactoryMethodDbEntityBuilder(LogModelFunc<Object> factoryMethod)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException("factoryMethod");
        }

        public LogModelFunc<Object> FactoryMethod => _factoryMethod;

        public Object BuildDbEntity(LogModel logData)
        {
            return _factoryMethod(logData);
        }
    }
}
