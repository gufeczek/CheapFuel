using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed record AddressDto(
    string? City, 
    string? Street, 
    string? StreetNumber, 
    string? PostalCode);

public sealed class AddressDtoProfile : Profile
{
    public AddressDtoProfile()
    {
        CreateMap<Address, AddressDto>();
    }
}