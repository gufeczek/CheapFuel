using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(AppDbContext context) : base(context) { }
}