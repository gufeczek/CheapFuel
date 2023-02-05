using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Tokens;
using MediatR;

namespace Application.Users.Commands.VerifyEmail;

public sealed class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    
    public VerifyEmailCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _emailVerificationTokenRepository = unitOfWork.EmailVerificationTokens;
        _userPrincipalService = userPrincipalService;
    }

    public async Task<Unit> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName() 
                       ?? throw new UnauthorizedException("User is not logged in!");
        var token = await _emailVerificationTokenRepository.GetUserToken(username) 
                    ?? throw new NotFoundException($"Not found email verification token for user {username}.");

        var expired = DateTime.UtcNow - token.CreatedAt > TimeSpan.FromHours(2);
        if (token.Count > 2 || expired)
        {
            throw new BadRequestException("Token has expired");
        }

        if (token.Token != request.Token)
        {
            await HandleInvalidToken(token);
        }

        token.User!.EmailConfirmed = true;
        _emailVerificationTokenRepository.Remove(token);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }

    private async Task HandleInvalidToken(EmailVerificationToken token)
    {
        token.Count += 1;
        await _unitOfWork.SaveAsync();

        throw new BadRequestException($"Invalid token. Number of attempts remaining {3 - token.Count}");
    }
}