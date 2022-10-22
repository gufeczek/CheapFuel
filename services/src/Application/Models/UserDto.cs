using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models;

public sealed class UserDto
{
    public string? Username { get; init; }
    public string? Email { get; init; }
    public string? EmailConfirmed { get; init; }
    public string? MultiFactorAuthEnabled { get; init; }
    public Role Role { get; init; }
    public AccountStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed class UserDtoProfile : Profile
{
    public UserDtoProfile()
    {
        CreateMap<User, UserDto>();
    }
}