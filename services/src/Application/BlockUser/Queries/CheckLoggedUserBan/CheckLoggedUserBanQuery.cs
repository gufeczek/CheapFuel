using Application.Models;
using MediatR;

namespace Application.BlockUser.Queries.CheckLoggedUserBan;

public sealed record CheckLoggedUserBanQuery() : IRequest<LoggedUserBanDto>{}
