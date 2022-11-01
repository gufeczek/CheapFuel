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

        builder.Property(e => e.Expired)
            .HasDefaultValue(false)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
    }
}