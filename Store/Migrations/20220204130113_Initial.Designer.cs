﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.Models;

#nullable disable

namespace Store.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220204130113_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Store.Models.Product", b =>
                {
                    b.Property<Guid>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductID = new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"),
                            Category = "Watersport",
                            Description = "A boat for one person",
                            Name = "Kayak",
                            Price = 5000m
                        },
                        new
                        {
                            ProductID = new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"),
                            Category = "Football",
                            Description = "A ball for paly footbal",
                            Name = "Football",
                            Price = 100m
                        },
                        new
                        {
                            ProductID = new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"),
                            Category = "Soccer",
                            Description = "A ball for paly soccer",
                            Name = "Soccer ball",
                            Price = 120.50m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
