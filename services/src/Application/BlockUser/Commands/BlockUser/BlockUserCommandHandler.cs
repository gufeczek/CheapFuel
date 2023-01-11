using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
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
        if(!await _userRepository.ExistsById(request.UserId!.Value))
        {
            throw new NotFoundException($"User not found for id = {request.UserId}");
        }
        
        if (await _blockUserRepository.ExistsByBlockedUserId((long)request.UserId!))
        {
            throw new ConflictException($"User with id = {request.UserId} has already banned");
        }

        var block = new BlockedUser
        {
            UserId = request.UserId,
            StartBanDate = DateTime.Now,
            EndBanDate = DateTime.Now.AddDays(5),
            Reason = request.Reason
        };
        
        _blockUserRepository.Add(block);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<BlockUserDto>(block);
    }
}