using Domain.Entities;
using Infrastructure.Persistence.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class FuelStationConfiguration : PermanentEntityConfiguration<FuelStation>
{
    public override void Configure(EntityTypeBuilder<FuelStation> builder)
    {
        base.Configure(builder);
        
        builder.Property(f => f.Name)
            .HasMaxLength(100)
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
            
        }).Navigation(f => f.Address).IsRequired();

        builder.OwnsOne(f => f.GeographicalCoordinates, g =>
        {
            g.WithOwner();

            g.Property(e => e.Latitude)
                .HasColumnName(nameof(GeographicalCoordinates.Latitude))
                .HasPrecision(10, 8)
                .IsRequired();

            g.Property(e => e.Longitude)
                .HasColumnName(nameof(GeographicalCoordinates.Longitude))
                .HasPrecision(11, 8)
                .IsRequired();
            
        }).Navigation(f => f.GeographicalCoordinates).IsRequired();
    }
}