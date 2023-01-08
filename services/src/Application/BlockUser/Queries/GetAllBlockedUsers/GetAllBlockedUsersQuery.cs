using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.BlockUser.Queries.GetAllBlockedUsers;

public sealed record GetAllBlockedUsersQuery(PageRequestDto PageRequestDto) : IRequest<Page<AllBlockedUsersDto>>;
