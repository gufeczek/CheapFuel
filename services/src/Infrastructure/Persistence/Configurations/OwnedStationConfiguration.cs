using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OwnedStationConfiguration : IEntityTypeConfiguration<OwnedStation>
{
    public void Configure(EntityTypeBuilder<OwnedStation> builder)
    {
        builder.HasKey(o => new { o.UserId, o.FuelStationId });
    }
}