using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class EmailVerificationTokenRepository : BaseRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(AppDbContext context) : base(context) { }
}