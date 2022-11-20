using Application.Models;
using Application.Models.Pagination;
using Domain.Common.Pagination.Response;
using MediatR;

namespace Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(PageRequestDto PageRequestDto) :  IRequest<Page<UserDetailsDto>>;

