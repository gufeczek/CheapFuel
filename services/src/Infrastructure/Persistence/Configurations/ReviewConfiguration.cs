using Domain.Entities;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : PermanentEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Rate)
            .HasPrecision(1, 0)
            .IsRequired();

        builder.Property(r => r.Content)
            .HasMaxLength(1000);

        builder.HasOne(r => r.FuelStation)
            .WithMany()
            .OnDelete(DeleteBehavior.ClientSetNull); // Should be change after changing FuelStation to permanent table
        
        builder.HasOne(r => r.User)
            .WithMany()
            .IsRequired();
    }
}