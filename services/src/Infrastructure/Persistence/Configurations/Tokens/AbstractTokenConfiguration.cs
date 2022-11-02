using Domain.Entities.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tokens;

public abstract class AbstractTokenConfiguration<T> : IEntityTypeConfiguration<T> where T : AbstractToken
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(e => e.Token)
            .HasMaxLength(6)
            .IsRequired();

        builder.Property(e => e.Count)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}