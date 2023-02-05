using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ReportedReviewConfiguration : IEntityTypeConfiguration<ReportedReview>
{
    public void Configure(EntityTypeBuilder<ReportedReview> builder)
    {
        builder.HasQueryFilter(o => o.User != null && !o.User.Deleted);
       
        builder.HasKey(o => new { o.UserId, o.ReviewId });

        builder.Property(r => r.ReportStatus)
            .HasConversion<string>()
            .IsRequired();


    }
}