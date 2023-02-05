using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IUserPrincipalService _userPrincipalService;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserPasswordHasher passwordHasher, 
        IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _passwordHasher = passwordHasher;
        _userPrincipalService = userPrincipalService;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetByUsernameAsync(username)
                   ?? throw new NotFoundException(nameof(User), nameof(User.Username), username);

        var isPasswordCorrect = _passwordHasher.IsPasswordCorrect(user.Password!, request.OldPassword, user);

        if (!isPasswordCorrect)
        {
            throw new UnauthorizedException("Invalid password");
        }

        var isPasswordSameAsOldOne = _passwordHasher.IsPasswordCorrect(user.Password!, request.NewPassword, user);

        if (isPasswordSameAsOldOne)
        {
            throw new BadRequestException("The new password cannot be the same as the old password");
        }
        
        var hashedPassword = _passwordHasher.HashPassword(request.NewPassword, user);
        user.Password = hashedPassword;

        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}