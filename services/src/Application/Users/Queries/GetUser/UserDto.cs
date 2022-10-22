using AutoMapper;
using Domain.Enums;

namespace Application.Users.Queries.GetUser;

public class UserDto
{
    public string? Username { get; init; }
    public Role Role { get; init; }
    public AccountStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Domain.Entities.User, UserDto>();
    }
}
