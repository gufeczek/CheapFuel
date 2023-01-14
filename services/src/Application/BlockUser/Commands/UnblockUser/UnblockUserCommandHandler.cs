using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.BlockUser.Commands.UnblockUser;

public class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlockUserRepository _blockUserRepository;
    private readonly IUserRepository _userRepository;

    public UnblockUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _blockUserRepository = unitOfWork.BlockedUsers;
        _userRepository = unitOfWork.Users;
    }

    public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username!) 
                   ?? throw new NotFoundException($"User not found for username = {request.Username}");

        var blockedUser = await _blockUserRepository.GetByBlockedUserId(user.Id) 
                         ?? throw new NotFoundException($"User with id = {user.Id} not found");
        
        _blockUserRepository.Remove(blockedUser);
        user.Status = AccountStatus.Active;
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}