using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.BlockUser.Commands.BlockUser;

public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, BlockUserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlockUserRepository _blockUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public BlockUserCommandHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _blockUserRepository = unitOfWork.BlockedUsers;
        _userRepository = unitOfWork.Users;
        _mapper = mapper;
    }

    public async Task<BlockUserDto> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username!) 
                   ?? throw new NotFoundException($"User not found for username = {request.Username}");
        
        if (await _blockUserRepository.ExistsByBlockedUserId(user.Id))
        {
            throw new ConflictException($"User with id = {user.Id} has already banned");
        }

        var block = new BlockedUser
        {
            UserId = user.Id,
            StartBanDate = DateTime.Now,
            EndBanDate = DateTime.Now.AddDays(5),
            Reason = request.Reason
        };
        user.Status = AccountStatus.Banned;

        _blockUserRepository.Add(block);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<BlockUserDto>(block);
    }
}