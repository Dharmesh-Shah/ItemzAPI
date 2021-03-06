﻿// <auto-generated />
using System;
using ItemzApp.API.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItemzApp.API.Migrations
{
    [DbContext(typeof(ItemzContext))]
    [Migration("20200417143802_ItemzMigration")]
    partial class ItemzMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ItemzApp.API.Entities.Itemz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1028)")
                        .HasMaxLength(1028);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Priority")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.ToTable("Itemzs");

                    b.HasData(
                        new
                        {
                            Id = new Guid("9153a516-d69e-4364-b17e-03b87442e21c"),
                            CreatedBy = "User 1",
                            CreatedDate = new DateTimeOffset(new DateTime(2019, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Description = "Requirements to be described here.",
                            Name = "Item 1",
                            Priority = "High",
                            Status = "New"
                        },
                        new
                        {
                            Id = new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783"),
                            CreatedBy = "User 2",
                            CreatedDate = new DateTimeOffset(new DateTime(2019, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Description = "Requirements to be described here.",
                            Name = "Item 2",
                            Priority = "Medium",
                            Status = "New"
                        },
                        new
                        {
                            Id = new Guid("b7ee9814-6556-429d-a0a1-69a3add2ddbe"),
                            CreatedBy = "User 3",
                            CreatedDate = new DateTimeOffset(new DateTime(2019, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Description = "Requirements to be described here.",
                            Name = "Item 3",
                            Priority = "Low",
                            Status = "New"
                        },
                        new
                        {
                            Id = new Guid("3475d7c0-d487-4e38-9b56-08b9e0ff6145"),
                            CreatedBy = "User 4",
                            CreatedDate = new DateTimeOffset(new DateTime(2019, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Description = "Requirements to be described here.",
                            Name = "Item 4",
                            Priority = "Low",
                            Status = "New"
                        },
                        new
                        {
                            Id = new Guid("05277468-9183-458d-96f3-eecb8d4296c1"),
                            CreatedBy = "User 5",
                            CreatedDate = new DateTimeOffset(new DateTime(2019, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 2, 0, 0, 0)),
                            Description = "Requirements to be described here.",
                            Name = "Item 5",
                            Priority = "Low",
                            Status = "New"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
