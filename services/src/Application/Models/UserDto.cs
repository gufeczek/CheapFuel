using AutoMapper;
using Domain.Enums;

namespace Application.Models;

public sealed class UserDto
{
    public string? Username { get; init; }
    public Role Role { get; init; }
    public AccountStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateMap<Domain.Entities.User, UserDto>();
    }
}
