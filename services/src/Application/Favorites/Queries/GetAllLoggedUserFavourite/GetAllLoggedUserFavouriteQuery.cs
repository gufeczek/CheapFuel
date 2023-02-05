using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.Favorites.Queries.GetAllLoggedUserFavourite;

public sealed record GetAllLoggedUserFavouriteQuery(PageRequestDto PageRequestDto) 
    : IRequest<Page<UserFavouriteDto>>;