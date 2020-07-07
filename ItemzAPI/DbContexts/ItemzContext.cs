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
        public DbSet<ProjectJoinItemz> ProjectJoinItemz { get; set; }
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
                    // Id = Guid.Parse("9B683E61-8EA0-429D-8B55-FFEECFCE11D0"),
                    Name = "Item 3",
                    Status = "New",
                    Priority = "Low",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 3",
                    CreatedDate = new DateTime(2019, 7, 03),
                },
                new Itemz()
                {
                    // Id = Guid.Parse("A64C64DE-5D3A-4534-93B4-55D813E71A5E"),
                    Name = "Item 4",
                    Status = "New",
                    Priority = "Low",
                    Description = "Requirements to be described here.",
                    CreatedBy = "User 4",
                    CreatedDate = new DateTime(2019, 7, 04),
                },
                new Itemz()
                {
                    // Id = Guid.Parse("0BB5FBCB-C166-4E2D-9008-B5FC2FAAB67B"),
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

            // EXPLANATION: Here we are defining a composite key for a join table.
            // it will use ProjectID + Itemz ID as it's composite key.

            modelBuilder.Entity<ProjectJoinItemz>()
                .HasKey(t => new { t.ProjectId, t.ItemzId });

            // EXPLANATION: Here we are defining Many to Many relationship between
            // Project and Itemz

            modelBuilder.Entity<ProjectJoinItemz>()
                .HasOne(p => p.Project)
                .WithMany(itemz => itemz.ProjectJoinItemz)
                .HasForeignKey(p => p.ProjectId);
            modelBuilder.Entity<ProjectJoinItemz>()
                .HasOne(itemz => itemz.Itemz)
                .WithMany(p => p.ProjectJoinItemz)
                .HasForeignKey(itemz => itemz.ItemzId);


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
            modelBuilder.Entity<ProjectJoinItemz>().HasData(
                 new ProjectJoinItemz()
                 {
                     ProjectId = new Guid("42f62a6c-fcda-4dac-a06c-406ac1c17770"),
                     ItemzId = new Guid("9153a516-d69e-4364-b17e-03b87442e21c")
                 },
                new ProjectJoinItemz()
                {
                    ProjectId = new Guid("b69cf0d7-70ad-4f73-aa4a-8daad5181e1e"),
                    ItemzId = new Guid("5e76f8e8-d3e7-41db-b084-f64c107c6783")
                }
                 );

            base.OnModelCreating(modelBuilder);
        }
    }
}