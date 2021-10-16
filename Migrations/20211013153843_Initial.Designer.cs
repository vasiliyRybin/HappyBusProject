﻿// <auto-generated />
using System;
using HappyBusProject;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HappyBusProject.Migrations
{
    [DbContext(typeof(MyShuttleBusAppNewDBContext))]
    [Migration("20211013153843_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HappyBusProject.Car", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RegistrationNumPlate")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("SeatsNum")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("HappyBusProject.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CarID");

                    b.Property<DateTime?>("MedicalExamPassDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("HappyBusProject.DriversRatingHistory", b =>
                {
                    b.Property<Guid>("RecordId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RecordID");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("DriverID");

                    b.Property<DateTime>("RatedWhenDateTime")
                        .HasColumnType("datetime");

                    b.Property<int>("RouteEndPointId")
                        .HasColumnType("int")
                        .HasColumnName("RouteEndPointID");

                    b.Property<int>("RouteStartPointId")
                        .HasColumnType("int")
                        .HasColumnName("RouteStartPointID");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.Property<Guid>("WhoRatedId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("WhoRatedID");

                    b.HasKey("RecordId")
                        .HasName("PK_DriversRatingHistory_1");

                    b.HasIndex("DriverId");

                    b.HasIndex("WhoRatedId");

                    b.ToTable("DriversRatingHistory");
                });

            modelBuilder.Entity("HappyBusProject.Order", b =>
                {
                    b.Property<Guid>("CarId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CarID");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CustomerID");

                    b.Property<int>("EndPointId")
                        .HasColumnType("int")
                        .HasColumnName("EndPointID");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<DateTime>("OrderDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("OrderType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("StartPointId")
                        .HasColumnType("int")
                        .HasColumnName("StartPointID");

                    b.HasKey("CarId", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HappyBusProject.RouteStop", b =>
                {
                    b.Property<int>("PointId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PointID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("PointId");

                    b.ToTable("RouteStops");
                });

            modelBuilder.Entity("HappyBusProject.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ID");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<bool>("IsInBlacklist")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HappyBusProject.UsersRatingHistory", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("UserID");

                    b.Property<string>("Comment")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("RatedWhenDateTime")
                        .HasColumnType("datetime");

                    b.Property<Guid>("RecordId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("RecordID");

                    b.Property<int>("RouteEndPointId")
                        .HasColumnType("int")
                        .HasColumnName("RouteEndPointID");

                    b.Property<int>("RouteStartPointId")
                        .HasColumnType("int")
                        .HasColumnName("RouteStartPointID");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UsersRatingHistory");
                });

            modelBuilder.Entity("HappyBusProject.Driver", b =>
                {
                    b.HasOne("HappyBusProject.Car", "Car")
                        .WithMany("Drivers")
                        .HasForeignKey("CarId")
                        .HasConstraintName("FK_Drivers_To_Cars")
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("HappyBusProject.DriversRatingHistory", b =>
                {
                    b.HasOne("HappyBusProject.Driver", "Driver")
                        .WithMany("DriversRatingHistories")
                        .HasForeignKey("DriverId")
                        .HasConstraintName("FK_DriversRatingHistory_Drivers")
                        .IsRequired();

                    b.HasOne("HappyBusProject.User", "WhoRated")
                        .WithMany("DriversRatingHistories")
                        .HasForeignKey("WhoRatedId")
                        .HasConstraintName("FK_DriversRatingHistory_Users")
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("WhoRated");
                });

            modelBuilder.Entity("HappyBusProject.Order", b =>
                {
                    b.HasOne("HappyBusProject.Car", "Car")
                        .WithMany("Orders")
                        .HasForeignKey("CarId")
                        .HasConstraintName("FK_Orders_Cars")
                        .IsRequired();

                    b.HasOne("HappyBusProject.User", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("FK_Orders_Users")
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HappyBusProject.UsersRatingHistory", b =>
                {
                    b.HasOne("HappyBusProject.User", "User")
                        .WithOne("UsersRatingHistory")
                        .HasForeignKey("HappyBusProject.UsersRatingHistory", "UserId")
                        .HasConstraintName("FK_UsersRatingHistory_Users")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HappyBusProject.Car", b =>
                {
                    b.Navigation("Drivers");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("HappyBusProject.Driver", b =>
                {
                    b.Navigation("DriversRatingHistories");
                });

            modelBuilder.Entity("HappyBusProject.User", b =>
                {
                    b.Navigation("DriversRatingHistories");

                    b.Navigation("Orders");

                    b.Navigation("UsersRatingHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
