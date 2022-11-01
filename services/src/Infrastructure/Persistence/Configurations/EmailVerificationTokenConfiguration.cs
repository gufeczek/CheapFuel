using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
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