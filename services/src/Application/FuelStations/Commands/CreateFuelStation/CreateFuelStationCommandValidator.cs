using Application.Models.FuelStationDtos;
using FluentValidation;

namespace Application.FuelStations.Commands.CreateFuelStation;

public sealed class CreateFuelStationCommandValidator : AbstractValidator<CreateFuelStationCommand>
{
    public CreateFuelStationCommandValidator()
    {
        RuleFor(c => c.FuelStationDto)
            .NotNull();

        When(x => x.FuelStationDto is not null, () =>
        {
            RuleFor(c => c.FuelStationDto!.Name)
                .MaximumLength(100);
        
            RuleFor(c => c.FuelStationDto!.Address)
                .NotNull()
                .SetValidator(new AddressDtoValidator());
        
            RuleFor(c => c.FuelStationDto!.GeographicalCoordinates)
                .NotNull();
        
            RuleFor(c => c.FuelStationDto!.GeographicalCoordinates)
                .SetValidator(new GeographicalCoordinatesDtoValidator());
        
            RuleFor(c => c.FuelStationDto!.StationChainId)
                .NotNull()
                .GreaterThanOrEqualTo(1);
        
            RuleFor(c => c.FuelStationDto!.FuelTypes)
                .NotNull();
        
            RuleForEach(c => c.FuelStationDto!.FuelTypes)
                .NotNull()
                .GreaterThanOrEqualTo(1);
        
            RuleFor(c => c.FuelStationDto!.Services)
                .NotNull();
        
            RuleForEach(c => c.FuelStationDto!.Services)
                .NotNull()
                .GreaterThanOrEqualTo(1);
        });
    }
}

public sealed class GeographicalCoordinatesDtoValidator : AbstractValidator<NewGeographicalCoordinatesDto>
{
    public GeographicalCoordinatesDtoValidator()
    {
        RuleFor(n => n.Latitude)
            .NotNull()
            .GreaterThanOrEqualTo(-90.0M)
            .LessThanOrEqualTo(90.0M);

        RuleFor(n => n.Longitude)
            .NotNull()
            .GreaterThanOrEqualTo(-180.0M)
            .LessThanOrEqualTo(180.0M);
    }
}

public sealed class AddressDtoValidator : AbstractValidator<NewAddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(n => n.City)
            .NotEmpty()
            .MaximumLength(50);
        
        RuleFor(n => n.Street)
            .MaximumLength(100);
        
        RuleFor(n => n.StreetNumber)
            .NotEmpty()
            .MaximumLength(10);
        
        RuleFor(n => n.PostalCode)
            .NotEmpty()
            .Length(6)
            .Matches("[0-9]{2}-[0-9]{3}");
    }
}