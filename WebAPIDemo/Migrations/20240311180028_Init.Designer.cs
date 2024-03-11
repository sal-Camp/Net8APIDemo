﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPIDemo.Data;

#nullable disable

namespace WebAPIDemo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240311180028_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebAPIDemo.Models.Shirt", b =>
                {
                    b.Property<int>("ShirtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShirtId"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("Size")
                        .HasColumnType("int");

                    b.HasKey("ShirtId");

                    b.ToTable("Shirts");

                    b.HasData(
                        new
                        {
                            ShirtId = 1,
                            Brand = "H&M",
                            Color = "Blue",
                            Gender = "women",
                            Price = 10.99,
                            Size = 6
                        },
                        new
                        {
                            ShirtId = 2,
                            Brand = "H&M",
                            Color = "Red",
                            Gender = "men",
                            Price = 12.99,
                            Size = 8
                        },
                        new
                        {
                            ShirtId = 3,
                            Brand = "AE",
                            Color = "Green",
                            Gender = "women",
                            Price = 14.99,
                            Size = 10
                        },
                        new
                        {
                            ShirtId = 4,
                            Brand = "AE",
                            Color = "Yellow",
                            Gender = "men",
                            Price = 16.989999999999998,
                            Size = 12
                        });
                });
#pragma warning restore 612, 618
        }
    }
}