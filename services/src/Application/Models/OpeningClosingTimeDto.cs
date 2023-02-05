using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class OpeningClosingTimeDto
{
    public DayOfWeek DayOfWeek { get; set; }
    public string? OpeningTime { get; set; }
    public string? ClosingTime { get; set; }    
}

public sealed class OpeningClosingTimeDtoProfile : Profile
{
    public OpeningClosingTimeDtoProfile()
    {
        CreateMap<OpeningClosingTime, OpeningClosingTimeDto>()
            .ForMember(
                d => d.OpeningTime,
                o => 
                    o.MapFrom<OpeningClosingTimeMemberResolver, int?>(s => s.OpeningTime))
            .ForMember(
                d => d.ClosingTime,
                o => 
                    o.MapFrom<OpeningClosingTimeMemberResolver, int?>(s => s.ClosingTime));

    }
}

public sealed class OpeningClosingTimeMemberResolver : IMemberValueResolver<OpeningClosingTime, OpeningClosingTimeDto, int?, string?> {
    
    public string? Resolve(OpeningClosingTime source, OpeningClosingTimeDto destination, int? sourceMember, string? destMember, ResolutionContext context)
    {
        return sourceMember is null 
            ? null 
            : sourceMember.ToString()!.PadLeft(4, '0').Insert(2, ":");
    }
}