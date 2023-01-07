using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class LoggedUserBanDto
{
    public long? UserId { get; set; }
    public DateTime StartBanDate { get; set; }
    public DateTime EndBanDate { get; set; }
    public string? Reason { get; set; }
}

public sealed class LoggedUserBanDtoProfile : Profile
{
    public LoggedUserBanDtoProfile()
    {
        CreateMap<BlockedUser, LoggedUserBanDto>();
    }
}