using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ServiceAtStationConfiguration : IEntityTypeConfiguration<ServiceAtStation>
{
    public void Configure(EntityTypeBuilder<ServiceAtStation> builder)
    {
        builder.HasKey(s => new { s.ServiceId, s.FuelStationId });
    }
}