using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FuelStationConfiguration : IEntityTypeConfiguration<FuelStation>
{
    public void Configure(EntityTypeBuilder<FuelStation> builder)
    {
        builder.Property(f => f.Name)
            .HasMaxLength(100);
        
        builder.Property(f => f.StationChainId)
            .IsRequired();

        builder.OwnsOne(f => f.Address, a =>
            {
                a.WithOwner();

                a.Property(e => e.City)
                    .HasColumnName(nameof(Address.City))
                    .HasMaxLength(50)
                    .IsRequired();

                a.Property(e => e.Street)
                    .HasColumnName(nameof(Address.Street))
                    .HasMaxLength(100);

                a.Property(e => e.StreetNumber)
                    .HasColumnName(nameof(Address.StreetNumber))
                    .HasMaxLength(10)
                    .IsRequired();

                a.Property(e => e.PostalCode)
                    .HasColumnName(nameof(Address.PostalCode))
                    .HasMaxLength(5)
                    .IsRequired();

            }).Navigation(f => f.Address)
            .IsRequired()
            .AutoInclude();

        builder.OwnsOne(f => f.GeographicalCoordinates, g =>
            {
                g.WithOwner();

                g.Property(e => e.Latitude)
                    .HasColumnName(nameof(GeographicalCoordinates.Latitude))
                    .HasPrecision(17, 15)
                    .IsRequired();

                g.Property(e => e.Longitude)
                    .HasColumnName(nameof(GeographicalCoordinates.Longitude))
                    .HasPrecision(17, 15)
                    .IsRequired();

            }).Navigation(f => f.GeographicalCoordinates)
            .IsRequired()
            .AutoInclude();
    }
}