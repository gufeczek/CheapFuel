using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OwnedStationConfiguration : IEntityTypeConfiguration<OwnedStation>
{
    public void Configure(EntityTypeBuilder<OwnedStation> builder)
    {
        builder.HasQueryFilter(o => o.User != null && !o.User.Deleted);
        
        builder.HasKey(o => new { o.UserId, o.FuelStationId });
    }
}