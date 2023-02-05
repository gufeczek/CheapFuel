using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelStationReviewDto
{
    public long Id { get; set; }
    public int Rate { get; set; }
    public string? Content { get; set; }
    public string? Username { get; set; }
    public long UserId { get; set; }
    public long FuelStationId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public sealed class FuelStationReviewDtoProfile : Profile
{
    public FuelStationReviewDtoProfile()
    {
        CreateMap<Review, FuelStationReviewDto>()
            .ForMember(
                d => d.Username, 
                o => o.MapFrom(s => s.User!.Username));
    }
}

public sealed class FuelStationReviewDtoColumnSelector : IColumnSelector<Review>
{
    public Dictionary<string, Expression<Func<Review, object>>> ColumnSelector { get; } = new()
    {
        { nameof(Review.CreatedAt), r => r.CreatedAt },
        { nameof(Review.UpdatedAt), r => r.UpdatedAt },
        { nameof(Review.Rate), r => r.Rate! }
    };
}