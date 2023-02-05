using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class AllBlockedUsersDto
{
    public long? UserId { get; set; }
    public DateTime StartBanDate { get; set; }
    public DateTime EndBanDate { get; set; }
    public string? Reason { get; set; }
}

public sealed class AllBlockedUsersDtoProfile : Profile
{
    public AllBlockedUsersDtoProfile()
    {
        CreateMap<BlockedUser, AllBlockedUsersDto>();
    }
}

public sealed class AllBlockedUsersDtoColumnSelector : IColumnSelector<BlockedUser>
{
    public Dictionary<string, Expression<Func<BlockedUser, object>>> ColumnSelector { get; } = new()
    {
        { nameof(BlockedUser.StartBanDate), r => r.StartBanDate },
        { nameof(BlockedUser.EndBanDate), r => r.EndBanDate },
    };
}