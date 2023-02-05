using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.BlockUser.Queries.CheckLoggedUserBan;

public sealed class CheckLoggedUserBanQueryHandler : IRequestHandler<CheckLoggedUserBanQuery, LoggedUserBanDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IBlockUserRepository _blockUserRepository;
    private readonly IMapper _mapper;

    public CheckLoggedUserBanQueryHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _blockUserRepository = unitOfWork.BlockedUsers;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<LoggedUserBanDto> Handle(CheckLoggedUserBanQuery checkLoggedUserBanQuery, CancellationToken cancellationToken)
    {
        var userId = _userPrincipalService.GetUserPrincipalId() ?? 
                     throw new UnauthorizedException("User not logged");
        if (await _blockUserRepository.ExistsByBlockedUserId(userId))
        {
            throw new NotFoundException("Your account is not banned");
        }
        var blockedUser = await _blockUserRepository.GetByBlockedUserId(userId);
        return _mapper.Map<LoggedUserBanDto>(blockedUser);
    }
}