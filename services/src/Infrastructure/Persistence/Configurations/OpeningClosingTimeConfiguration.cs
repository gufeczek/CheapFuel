using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class OpeningClosingTimeConfiguration : IEntityTypeConfiguration<OpeningClosingTime>
{
    public void Configure(EntityTypeBuilder<OpeningClosingTime> builder)
    {
        builder.HasKey(o => new { o.FuelStationId, o.DayOfWeek });

        builder.HasOne(o => o.FuelStation)
            .WithMany(f => f.OpeningClosingTimes)
            .HasForeignKey(o => o.FuelStationId);

        builder.Property(o => o.OpeningTime)
            .HasPrecision(4, 0)
            .IsRequired();

        builder.Property(o => o.ClosingTime)
            .HasPrecision(4, 0)
            .IsRequired();
    }
}