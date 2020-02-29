using Microsoft.EntityFrameworkCore;

namespace TacitusLogger.Destinations.EntityFramework
{
    public interface IDbContextProvider
    {
        DbContext GetDbContext(LogModel logData);
    }
}
