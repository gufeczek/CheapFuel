using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class StationChainConfiguration : IEntityTypeConfiguration<StationChain>
{
    public void Configure(EntityTypeBuilder<StationChain> builder)
    {
        builder.Property(s => s.Name)
            .HasMaxLength(128)
            .IsRequired();
    }
}