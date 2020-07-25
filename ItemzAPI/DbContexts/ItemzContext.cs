// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ItemzApp.API.DbContexts
{
    public class ItemzContext : DbContext
    {
        public ItemzContext(DbContextOptions<ItemzContext> options) : base(options)
        {

        }
        public DbSet<Itemz> Itemzs { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<ProjectJoinItemz> ProjectJoinItemz { get; set; }

        public DbSet<ItemzType> ItemzTypes { get; set; }
        public DbSet<ItemzTypeJoinItemz> ItemzTypeJoinItemz { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // seed the database with dummy data
            modelBuilder.Entity<Itemz>().HasData(
                new Itemz()
                {
                    Id = Guid.Parse("9153a516-d69e-4364-b17e-03b87442e21c"),
                    Name = "Item 1",
                    Status = "New",
                    Priority = "High",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                },
                new Itemz()
                {
                    Id = Guid.Parse("5e76f8e8-d3e7-41db-b084-f64c107c6783"),
                    Name = "Item 2",
                    Status = "New",
                    Priority = "Medium",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 2",
                    CreatedDate = new DateTime(2019, 7, 02),
                },
                new Itemz()
                {
                    Id = Guid.Parse("885b8e56-ffe6-4bc9-82e2-ce23230991db"),
                    Name = "Item 3",
                    Status = "New",
                    Priority = "Low",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 3",
                    CreatedDate = new DateTime(2019, 7, 03),
                },
                new Itemz()
                {
                    Id = Guid.Parse("0850bc8a-84c4-4c52-8ab6-b06e7bc2195b"),
                    Name = "Item 4",
                    Status = "New",
                    Priority = "Low",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 4",
                    CreatedDate = new DateTime(2019, 7, 04),
                },
                new Itemz()
                {
                    Id = Guid.Parse("4adde56d-8081-45ea-bd37-5c830b63873b"),
                    Name = "Item 5",
                    Status = "New",
                    Priority = "Low",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 5",
                    CreatedDate = new DateTime(2019, 7, 05),
                }
            );

            // EXPLANATION: This will make sure that GUID property is set to autogenerate in the 
            // SQL Server Database as well.

            modelBuilder.Entity<Itemz>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Itemz>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            // EXPLANATION: This will make sure that GUID property is set to autogenerate in the 
            // SQL Server Database as well.

            modelBuilder.Entity<Project>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            // EXPLANATION: This way, we are adding Unique Index for Project Name.
            // It's not possible to use Attribute for creating Unique Index in
            // EF Core 3.1. That is why I'm using Fluent API for the same.
            // Docs ca be found at ...
            // https://docs.microsoft.com/en-us/ef/core/modeling/indexes
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Name)
                .IsUnique();

            //// EXPLANATION: Here we are defining a composite key for a join table.
            //// it will use ProjectID + Itemz ID as it's composite key.

            //modelBuilder.Entity<ProjectJoinItemz>()
            //    .HasKey(t => new { t.ProjectId, t.ItemzId });

            //// EXPLANATION: Here we are defining Many to Many relationship between
            //// Project and Itemz

            //modelBuilder.Entity<ProjectJoinItemz>()
            //    .HasOne(p => p.Project)
            //    .WithMany(itemz => itemz.ProjectJoinItemz)
            //    .HasForeignKey(p => p.ProjectId);
            //modelBuilder.Entity<ProjectJoinItemz>()
            //    .HasOne(itemz => itemz.Itemz)
            //    .WithMany(p => p.ProjectJoinItemz)
            //    .HasForeignKey(itemz => itemz.ItemzId);


            modelBuilder.Entity<Project>().HasData(
                new Project()
                {
                    Id = Guid.Parse("42f62a6c-fcda-4dac-a06c-406ac1c17770"),
                    Name = "Project 1",
                    Status = "Active",
                    Description = "This is Project 1",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                },
                new Project()
                {
                    Id = Guid.Parse("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"),
                    Name = "Project 2",
                    Status = "Active",
                    Description = "This is Project 2",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                }
                );
            //modelBuilder.Entity<ProjectJoinItemz>().HasData(
            //     new ProjectJoinItemz()
            //     {
            //         ProjectId = new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"),
            //         ItemzId = new Guid("9153a516-d69e-4364-b17e-03b87442e21c")
            //     },
            //    new ProjectJoinItemz()
            //    {
            //        ProjectId = new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"),
            //        ItemzId = new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783")
            //    }
            //     );








            // EXPLANATION: This will make sure that GUID property is set to autogenerate in the 
            // SQL Server Database as well.

            modelBuilder.Entity<ItemzType>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<ItemzType>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            // EXPLANATION: Here we are defining a composite key for a join table.
            // it will use ItemzTypeID + Itemz ID as it's composite key.

            modelBuilder.Entity<ItemzTypeJoinItemz>()
                .HasKey(t => new { t.ItemzTypeId, t.ItemzId });

            // EXPLANATION: Here we are defining Many to Many relationship between
            // ItemzType and Itemz

            modelBuilder.Entity<ItemzTypeJoinItemz>()
                .HasOne(itji => itji.ItemzType)
                .WithMany(it => it.ItemzTypeJoinItemz)
                .HasForeignKey(itji => itji.ItemzTypeId);
            modelBuilder.Entity<ItemzTypeJoinItemz>()
                .HasOne(itji => itji.Itemz)
                .WithMany(i => i.ItemzTypeJoinItemz)
                .HasForeignKey(itji => itji.ItemzId);

            modelBuilder.Entity<ItemzType>()
                .HasOne(it => it.Project)
                .WithMany(p => p.ItemzTypes)
                .HasForeignKey(it => it.ProjectId);

            modelBuilder.Entity<ItemzType>().HasData(
                new ItemzType()
                {
                    Id = Guid.Parse("473fe535-1420-42cf-8a40-224388b8df24"),
                    Name = "Parking Lot",
                    Status = "Active",
                    Description = "This is Parking Lot system ItemzType",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                    ProjectId = Guid.Parse("42f62a6c-fcda-4dac-a06c-406ac1c17770")
                },
                new ItemzType()
                {
                    Id = Guid.Parse("611639db-577a-48f6-9b08-f6aef477368f"),
                    Name = "ItemzType 1",
                    Status = "Active",
                    Description = "This is ItemzType 1",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                    ProjectId = Guid.Parse("42f62a6c-fcda-4dac-a06c-406ac1c17770")
                },
                new ItemzType()
                {
                    Id = Guid.Parse("1a069648-6ad6-4c8a-be05-be747bdeb8da"),
                    Name = "Parking Lot",
                    Status = "Active",
                    Description = "This is Parking Lot system ItemzType",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                    ProjectId = Guid.Parse("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e")
                },
                new ItemzType()
                {
                    Id = Guid.Parse("8414bf58-6331-4b3e-bbf0-f780950a337b"),
                    Name = "ItemzType 2",
                    Status = "Active",
                    Description = "This is ItemzType 2",
                    CreatedBy = "User 1",
                    CreatedDate = new DateTime(2019, 7, 01),
                    ProjectId = Guid.Parse("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e")
                }
            );
            modelBuilder.Entity<ItemzTypeJoinItemz>().HasData(
                 new ItemzTypeJoinItemz()
                 {
                     ItemzTypeId = new Guid("611639db-577a-48f6-9b08-f6aef477368f"),
                     ItemzId = new Guid("9153a516-d69e-4364-b17e-03b87442e21c")
                 },
                new ItemzTypeJoinItemz()
                {
                    ItemzTypeId = new Guid("8414bf58-6331-4b3e-bbf0-f780950a337b"),
                    ItemzId = new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783")
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}