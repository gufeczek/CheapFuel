using Domain.Entities;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FuelPriceConfiguration : PermanentEntityConfiguration<FuelPrice>
{
    public override void Configure(EntityTypeBuilder<FuelPrice> builder)
    {
        base.Configure(builder);

        builder.Property(f => f.Price)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(f => f.Available)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(f => f.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(f => f.Priority)
            .IsRequired();

        builder.HasOne(f => f.FuelStation)
            .WithMany(s => s.FuelPrices)
            .HasForeignKey(f => f.FuelStationId)
            .OnDelete(DeleteBehavior.ClientSetNull); // Should be change after changing FuelStation to permanent table

        builder.HasOne(f => f.FuelType)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull); // Should be change after changing FuelType to permanent table

        builder.HasOne(f => f.User)
            .WithMany()
            .IsRequired();

        builder.HasIndex(f => new
            {
                f.Status, 
                f.FuelTypeId,
                f.Available,
                f.Price,
            })
            .IsUnique(false);

        builder.HasIndex(f => f.CreatedAt)
            .IsUnique(false);
    }
}