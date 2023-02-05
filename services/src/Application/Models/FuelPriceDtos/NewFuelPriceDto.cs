namespace Application.Models.FuelPriceDtos;

public sealed class NewFuelPriceDto
{
    public decimal? Price { get; set; }
    public bool? Available { get; set; }
    public long? FuelTypeId { get; set; }
}