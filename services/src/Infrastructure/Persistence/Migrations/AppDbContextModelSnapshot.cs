﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Entities.FuelStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("DeletedBy")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("FuelStations", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.FuelType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.ToTable("FuelTypes", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.OpeningClosingTime", b =>
                {
                    b.Property<long>("FuelStationId")
                        .HasColumnType("bigint");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int?>("ClosingTime")
                        .IsRequired()
                        .HasPrecision(4)
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("CreatedBy")
                        .HasColumnType("bigint");

                    b.Property<int?>("OpeningTime")
                        .IsRequired()
                        .HasPrecision(4)
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("UpdatedBy")
                        .HasColumnType("bigint");

                    b.HasKey("FuelStationId", "DayOfWeek");

                    b.ToTable("OpeningClosingTime", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Service", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.ToTable("Services", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.StationChain", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.ToTable("StationChains", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool?>("EmailConfirmed")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<bool?>("MultiFactorAuthEnabled")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("varchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.FuelStation", b =>
                {
                    b.OwnsOne("Domain.Entities.FuelStation.Address#Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<long>("FuelStationId")
                                .HasColumnType("bigint");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("varchar(50)")
                                .HasColumnName("City");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasMaxLength(5)
                                .HasColumnType("varchar(5)")
                                .HasColumnName("PostalCode");

                            b1.Property<string>("Street")
                                .HasMaxLength(100)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Street");

                            b1.Property<string>("StreetNumber")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("varchar(10)")
                                .HasColumnName("StreetNumber");

                            b1.HasKey("FuelStationId");

                            b1.ToTable("FuelStations", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("FuelStationId");
                        });

                    b.OwnsOne("Domain.Entities.FuelStation.GeographicalCoordinates#Domain.Entities.GeographicalCoordinates", "GeographicalCoordinates", b1 =>
                        {
                            b1.Property<long>("FuelStationId")
                                .HasColumnType("bigint");

                            b1.Property<decimal>("Latitude")
                                .HasPrecision(10, 8)
                                .HasColumnType("decimal(10,8)")
                                .HasColumnName("Latitude");

                            b1.Property<decimal>("Longitude")
                                .HasPrecision(11, 8)
                                .HasColumnType("decimal(11,8)")
                                .HasColumnName("Longitude");

                            b1.HasKey("FuelStationId");

                            b1.ToTable("FuelStations", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("FuelStationId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("GeographicalCoordinates")
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.OpeningClosingTime", b =>
                {
                    b.HasOne("Domain.Entities.FuelStation", "FuelStation")
                        .WithMany("OpeningClosingTimes")
                        .HasForeignKey("FuelStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FuelStation");
                });

            modelBuilder.Entity("Domain.Entities.FuelStation", b =>
                {
                    b.Navigation("OpeningClosingTimes");
                });
#pragma warning restore 612, 618
        }
    }
}
