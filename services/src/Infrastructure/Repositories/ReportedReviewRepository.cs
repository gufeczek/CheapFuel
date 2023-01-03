using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReportedReviewRepository : Repository<ReportedReview>, IReportedReviewRepository
{
    public ReportedReviewRepository(AppDbContext context) : base(context) { }
    
    public async Task<bool> ExistsByReviewAndUserId(long reviewId, long userId)
    {
        return await Context.ReportedReviews
            .Where(r => r.UserId == userId && r.ReviewId == reviewId)
            .AnyAsync();
    }

    public async Task<ReportedReview?> GetByReviewIdAndUserId(long reviewId, long userId)
    {
        return await Context.ReportedReviews
            .Where(r => r.UserId == userId && r.ReviewId == reviewId)
            .FirstOrDefaultAsync();
    }
    
    public async Task<ReportedReview?> GetByReviewId(long reviewId)
    {
        return await Context.ReportedReviews
            .Where(r =>r.ReviewId == reviewId)
            .FirstOrDefaultAsync();
    }

}