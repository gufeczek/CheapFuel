using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using MediatR;

namespace Application.Users.Commands.GenerateEmailVerificationToken;

public sealed class GenerateEmailVerificationTokenCommandHandler : IRequestHandler<GenerateEmailVerificationTokenCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly ITokenService _tokenService;
    private readonly IEmailSenderService _emailSenderService;
    
    public GenerateEmailVerificationTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IUserPrincipalService userPrincipalService,
        ITokenService tokenService,
        IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _emailVerificationTokenRepository = unitOfWork.EmailVerificationTokens;
        _userPrincipalService = userPrincipalService;
        _tokenService = tokenService;
        _emailSenderService = emailSenderService;
    }


    public async Task<Unit> Handle(GenerateEmailVerificationTokenCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username)
                   ?? throw new NotFoundException(nameof(User), nameof(User.Username), username);

        if ((bool)user.EmailConfirmed!)
        {
            throw new BadRequestException("Email is already verified");
        }
        
        var tokenCode = _tokenService.GenerateSimpleToken();
        var token = new EmailVerificationToken
        {
            Token = tokenCode,
            Count = 0,
            User = user
        };
        
        await _emailVerificationTokenRepository.RemoveAllByUsername(username);
        _emailVerificationTokenRepository.Add(token);
        await _unitOfWork.SaveAsync();
        await _emailSenderService.SendEmailAddressVerificationToken(user.Email!, tokenCode);
        
        return Unit.Value;
    }
}