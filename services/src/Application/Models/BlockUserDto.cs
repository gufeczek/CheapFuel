using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models;

public sealed class BlockUserDto
{
    public long? UserId { get; set; }
    public string? Reason { get; set; }
}

public sealed class BlockUserDtoProfile : Profile
{
    public BlockUserDtoProfile()
    {   
        CreateMap<BlockedUser, BlockUserDto>();
    }
}