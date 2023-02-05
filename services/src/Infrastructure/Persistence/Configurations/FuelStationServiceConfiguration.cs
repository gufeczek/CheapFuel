using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FuelStationServiceConfiguration : IEntityTypeConfiguration<FuelStationService>
{
    public void Configure(EntityTypeBuilder<FuelStationService> builder)
    {
        builder.ToTable("Services");
        
        builder.Property(s => s.Name)
            .HasMaxLength(32)
            .IsRequired();
    }
}