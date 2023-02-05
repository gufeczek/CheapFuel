using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Common;

public abstract class PermanentEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : PermanentEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasQueryFilter(p => !p.Deleted);

        builder.Property(p => p.Deleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}