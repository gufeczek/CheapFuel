using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.BlockUser.Queries.CheckLoggedUserBan;

public sealed class CheckLoggedUserBanQueryHandler : IRequestHandler<CheckLoggedUserBanQuery, string>
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
    
    public async Task<string> Handle(CheckLoggedUserBanQuery checkLoggedUserBanQuery, CancellationToken cancellationToken)
    {
        var userId = _userPrincipalService.GetUserPrincipalId() ?? 
                     throw new UnauthorizedException("User not logged");
        var user = await _userRepository.GetAsync(userId) ??
                     throw new NotFoundException(nameof(User), nameof(User.Username), userId.ToString());
        if (await _blockUserRepository.ExistsByBlockedUserId(userId))
        {
            return "Your account is not banned :)";
        }
        var blockedUser = await _blockUserRepository.GetByBlockedUserId(userId);
        return $"Your account is blocked since = {blockedUser.StartBanDate} to {blockedUser.EndBanDate}";

    }
}