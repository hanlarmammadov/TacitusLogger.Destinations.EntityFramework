using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TacitusLogger.Destinations.EntityFramework.IntegrationTests
{
    public class LogModelConfiguration : IEntityTypeConfiguration<LogDbModel>
    {
        public void Configure(EntityTypeBuilder<LogDbModel> builder)
        {
            builder.HasKey(x => x.LogId);
        }
    }
}
