using Application.Common.Authentication;
using Application.Common.Interfaces;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using MediatR;

namespace Application.Users.Commands.GeneratePasswordResetToken;

public sealed class GeneratePasswordResetTokenCommandHandler : IRequestHandler<GeneratePasswordResetTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly IEmailSenderService _emailSenderService;

    public GeneratePasswordResetTokenCommandHandler(
        IUnitOfWork unitOfWork, 
        ITokenService tokenService, 
        IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _passwordResetTokenRepository = unitOfWork.PasswordResetTokenRepository;
        _tokenService = tokenService;
        _emailSenderService = emailSenderService;
    }

    public async Task<Unit> Handle(GeneratePasswordResetTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAddressAsync(request.Email);

        if (user is null)
        {
            return Unit.Value;
        }

        var tokenCode = _tokenService.GenerateSimpleToken();
        var token = new PasswordResetToken
        {
            Token = tokenCode,
            Count = 0,
            User = user
        };

        await _passwordResetTokenRepository.RemoveAllByUsername(user.Username!);
        _passwordResetTokenRepository.Add(token);
        await _unitOfWork.SaveAsync();
        await _emailSenderService.SendPasswordResetToken(user.Email!, tokenCode);
        
        return Unit.Value;
    }
}