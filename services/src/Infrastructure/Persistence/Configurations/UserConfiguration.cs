using Domain.Entities;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserConfiguration : PermanentEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        builder.Property(u => u.Username)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(u => u.EmailConfirmed)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.MultiFactorAuthEnabled)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .IsRequired();
    }
}