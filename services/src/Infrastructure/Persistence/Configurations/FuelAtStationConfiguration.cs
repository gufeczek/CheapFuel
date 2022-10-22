using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FuelAtStationConfiguration : IEntityTypeConfiguration<FuelAtStation>
{
    public void Configure(EntityTypeBuilder<FuelAtStation> builder)
    {
        builder.HasKey(f => new { f.FuelTypeId, f.FuelStationId });
    }
}