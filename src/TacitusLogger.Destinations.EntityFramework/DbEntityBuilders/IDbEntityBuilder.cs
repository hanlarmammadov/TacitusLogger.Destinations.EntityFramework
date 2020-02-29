using System;

namespace TacitusLogger.Destinations.EntityFramework
{
    public interface IDbEntityBuilder
    {
        Object BuildDbEntity(LogModel logData);
    }
}
