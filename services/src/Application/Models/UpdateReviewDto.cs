using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class UpdateReviewDto
{
    public int? Rate { get; set; }
    public string? Content { get; set; }
}

public sealed class NewReviewDtoProfile : Profile
{
    public NewReviewDtoProfile()
    {
        CreateMap<UpdateReviewDto, Review>();
    }
}
