using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Users.Queries.GetLoggedUser;

public sealed class GetLoggedUserQueryHandler : IRequestHandler<GetLoggedUserQuery, UserDetailsDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserPrincipalService _userPrincipalService;

    public GetLoggedUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserPrincipalService userPrincipalService)
    {
        _userRepository = unitOfWork.Users;
        _mapper = mapper;
        _userPrincipalService = userPrincipalService;
    }

    public async Task<UserDetailsDto> Handle(GetLoggedUserQuery getLoggedUserQuery,  CancellationToken cancellationToken)
    {
        var userId = _userPrincipalService.GetUserPrincipalId() ?? throw new UnauthorizedException("User not authorized to perform this");
        var user = await _userRepository.GetAsync(userId)
                   ?? throw new NotFoundException(nameof(User), nameof(User.Username), userId.ToString());

        return _mapper.Map<UserDetailsDto>(user);
    }
}
