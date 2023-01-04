using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDetailsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IMapper _mapper;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IEmailSenderService _emailSenderService;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IUserPasswordHasher passwordHasher, 
        ITokenService tokenService,
        IEmailSenderService emailSenderService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _emailVerificationTokenRepository = unitOfWork.EmailVerificationTokens;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _emailSenderService = emailSenderService;
    }
    
    public async Task<UserDetailsDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsername(request.Username))
        {
            throw new DuplicateCredentialsException("Username is already taken");
        }

        if (await _userRepository.ExistsByEmail(request.Email))
        {
            throw new DuplicateCredentialsException("Email is already taken");
        }

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false,
            Status = AccountStatus.Active,
            Role = Role.User
        };

        var hashedPassword = _passwordHasher.HashPassword(request.Password, newUser);
        newUser.Password = hashedPassword;
        
        _userRepository.Add(newUser);
        await _unitOfWork.SaveAsync();

        await SendVerificationTokenToUser(newUser);
        
        return _mapper.Map<UserDetailsDto>(newUser);
    }

    private async Task SendVerificationTokenToUser(User user)
    {
        var tokenCode = _tokenService.GenerateSimpleToken();
        var token = new EmailVerificationToken
        {
            Token = tokenCode,
            Count = 0,
            User = user
        };
        _emailVerificationTokenRepository.Add(token);
        //await _unitOfWork.SaveAsync();
        
        await _emailSenderService.SendEmailAddressVerificationToken(user.Email!, tokenCode);
    }
}