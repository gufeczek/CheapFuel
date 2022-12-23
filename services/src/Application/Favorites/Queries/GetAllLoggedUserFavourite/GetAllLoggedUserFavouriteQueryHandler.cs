using Application.Common;
using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Favorites.Queries.GetAllLoggedUserFavourite;

public sealed class GetAllLoggedUserFavouriteQueryHandler : IRequestHandler<GetAllLoggedUserFavouriteQuery, Page<UserFavouriteDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public GetAllLoggedUserFavouriteQueryHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _favoriteRepository = unitOfWork.Favorites;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }

    public async Task<Page<UserFavouriteDto>> Handle(GetAllLoggedUserFavouriteQuery request, CancellationToken cancellationToken)
    {
        var username = _userPrincipalService.GetUserName()
                       ?? throw new UnauthorizedException("User is not logged in!");

        if (!await _userRepository.ExistsByUsername(username))
        {
            throw new NotFoundException($"User not found for username = {username}");
        }
        
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new UserFavouriteDtoColumnSelector());
        var result = await _favoriteRepository.GetAllByUsernameAsync(username, pageRequest);
        return Page<UserFavouriteDto>.From(result, _mapper.Map<IEnumerable<UserFavouriteDto>>(result.Data));
    }
}