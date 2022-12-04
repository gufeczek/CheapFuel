using Application.Common.Authentication;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Commands.DeactivateUser;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;

    public DeactivateUserCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _userPrincipalService = userPrincipalService;
    }
    
    public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName() 
                       ?? throw new UnauthorizedException("User is not logged in!");
        var loggedUser = await _userRepository.GetByUsernameAsync(username) 
                         ?? throw new NotFoundException($"User not found for username = {username}");
        var userToRemove = loggedUser;
        
        if (!loggedUser.Username!.Equals(request.Username))
        {
            userToRemove = await _userRepository.GetByUsernameAsync(request.Username!)
                           ?? throw new NotFoundException($"User not found for username = {username}");
        }

        if (!loggedUser.Username.Equals(request.Username) && loggedUser.Role != Role.Admin)
        {
            throw new ForbiddenException("You are not authorized to perform this action");
        }
        
        _userRepository.Remove(userToRemove);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}