using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using MediatR;

namespace Application.Users.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly IUserPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        IUnitOfWork unitOfWork, 
        IPasswordResetTokenRepository passwordResetTokenRepository, 
        IUserPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAddressAsync(request.Email);

        if (user is null)
        {
            throw new BadRequestException("Invalid token");
        }

        var token = await _passwordResetTokenRepository.GetUserToken(user.Username!)
                    ?? throw new BadRequestException("Invalid token");

        var expired = DateTime.UtcNow - token.CreatedAt > TimeSpan.FromMinutes(30);
        if (token.Count > 2 || expired)
        {
            throw new BadRequestException("Token has expired");
        }

        if (token.Token != request.Token)
        {
            await HandleInvalidToken(token);
        }

        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword, user);
        user.Password = hashedPassword;

        _passwordResetTokenRepository.Remove(token);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }

    private async Task HandleInvalidToken(PasswordResetToken token)
    {
        token.Count += 1;
        await _unitOfWork.SaveAsync();
        
        throw new BadRequestException($"Invalid token. Number of attempts remaining {3 - token.Count}");
    }
}