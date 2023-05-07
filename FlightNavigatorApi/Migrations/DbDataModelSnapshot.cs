﻿// <auto-generated />
using System;
using FlightNavigatorApi.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightNavigatorApi.Migrations
{
    [DbContext(typeof(DbData))]
    partial class DbDataModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlightNavigatorApi.Model.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsArrival")
                        .HasColumnType("bit");

                    b.Property<int>("Leg")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Flight");
                });

            modelBuilder.Entity("FlightNavigatorApi.Model.FlightLogger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DateFlight")
                        .HasColumnType("datetime2");

                    b.Property<int>("FlightId")
                        .HasColumnType("int");

                    b.Property<int>("FromLeg")
                        .HasColumnType("int");

                    b.Property<int>("ToLeg")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.ToTable("FlightLogger");
                });

            modelBuilder.Entity("FlightNavigatorApi.Model.FlightLogger", b =>
                {
                    b.HasOne("FlightNavigatorApi.Model.Flight", null)
                        .WithMany("FlightLoggers")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlightNavigatorApi.Model.Flight", b =>
                {
                    b.Navigation("FlightLoggers");
                });
#pragma warning restore 612, 618
        }
    }
}
