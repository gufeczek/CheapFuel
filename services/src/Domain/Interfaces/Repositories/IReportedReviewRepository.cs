using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IReportedReviewRepository : IRepository<ReportedReview>
{
    Task<bool> ExistsByReviewAndUserId(long reviewId, long userId);

    Task<ReportedReview?> GetByReviewIdAndUserId(long reviewId, long userId);

    Task<ReportedReview?> GetByReviewId(long reviewId);
}

