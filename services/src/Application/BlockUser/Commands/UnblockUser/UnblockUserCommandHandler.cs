using Application.Common.Exceptions;
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
        if(!await _userRepository.ExistsById(request.UserId!.Value))
        {
            throw new NotFoundException($"User not found for id = {request.UserId}");
        }

        var blockedUser = await _blockUserRepository.GetByBlockedUserId(request.UserId!.Value) 
                         ?? throw new NotFoundException($"User with id = {request.UserId} not found");
        
        _blockUserRepository.Remove(blockedUser);
        await _unitOfWork.SaveAsync();
        
        return Unit.Value;
    }
}