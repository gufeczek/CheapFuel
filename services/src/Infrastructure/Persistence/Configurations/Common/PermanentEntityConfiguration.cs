using Domain.Common;
using Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common;

public abstract class PermanentEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : PermanentEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(p => p.Deleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}