// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.DbContexts.Interceptors;
using ItemzApp.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ItemzApp.API.DbContexts
{
    public class ItemzContext : DbContext
    {
        //private readonly ItemzContexInterceptor _itemzContexInterceptor = new ItemzContexInterceptor();
      
        public ItemzContext(DbContextOptions<ItemzContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            bool IsItemzContexInterceptorRegisteredAlready = false;

            if (optionsBuilder.IsConfigured)
            {
                   // TODO: for some reason, this is always true here. Investigate why
                   // EF Core team has provided this property and what is the real use 
                   // of the same.
            }
            //#region Register_ItemzContexInterceptor_Only_Once
            //// EXPLANATION: We don't want to register Interceptor more then once  
            //// for capturing Audit Entries. Interceptors can be registered as options for 
            //// services.AddDBContext within Startup.cs file. This is considered as Global 
            //// registration and it's kind of one time registration for a given DBContext.
            //// Now if we register Interceptor yet again via overriding OnConfiguring within
            //// concrete implementation of DBContext (as you see here in this example) then 
            //// we are actually registering the same interceptor twice. That means the 
            //// SavedChanges, SavedChangesAsync, SavingChanges, SavingChangesAsync, etc. 
            //// methods will be called twice (or as many times as we have registered DBContext).
            //// In fact, each time user creates instance of concrete implementation of DBContext,
            //// it will go to the ServiceProvider and get a new instance as DBContext are by default,
            //// designed to be registered in DI Container as Scoped service. 
            //// In following code, we are checking if a Global registration is already done 
            //// in services.AddDBContext within Startup.cs then we do not register a second 
            //// instance of Interceptor while creating a new instance of DBContext which is part of the 
            //// scoped service.
            //// Ref: Microsoft.EntityFrameworkCore.Diagnostics.ISaveChangesInterceptor
            //// Ref: Microsoft.EntityFrameworkCore.Diagnostics.IInterceptor

            //foreach (var extension in optionsBuilder.Options.Extensions)
            //{
            //    if (extension.GetType().Equals(typeof(Microsoft.EntityFrameworkCore.Infrastructure.CoreOptionsExtension)))
            //    {
            //        if ((((Microsoft.EntityFrameworkCore.Infrastructure.CoreOptionsExtension)extension)
            //                                    .Interceptors) is null)
            //        {
            //            break;
            //        }
            //        foreach (var interceptor in (((Microsoft.EntityFrameworkCore.Infrastructure.CoreOptionsExtension)extension).Interceptors))
            //        {
            //            if (interceptor.GetType().Equals(typeof(ItemzContexInterceptor)))
            //            {
            //                IsItemzContexInterceptorRegisteredAlready = true;
            //                break;
            //            }
            //        }
            //    }
            //    if (IsItemzContexInterceptorRegisteredAlready)
            //    {
            //        break;
            //    }
            //}
            //if (!IsItemzContexInterceptorRegisteredAlready)
            //{
            //    optionsBuilder.AddInterceptors(_itemzContexInterceptor);
            //}
            //#endregion Register_ItemzContexInterceptor_Only_Once
        }

        public DbSet<Itemz> Itemzs { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<ProjectJoinItemz> ProjectJoinItemz { get; set; }

        public DbSet<ItemzType> ItemzTypes { get; set; }
        public DbSet<ItemzTypeJoinItemz> ItemzTypeJoinItemz { get; set; }
        public DbSet<ItemzChangeHistory> ItemzChangeHistory { get; set; }

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

            // EXPLANATION: Severity is controlled by build in ENUM which has a fixed 
            // list of values. ItemzAPP API is going to control this values but 
            // by design we are only storing string values into the database
            // for selected Severity Value. 

            modelBuilder.Entity<Itemz>()
                 .Property(i => i.Severity)
              .HasMaxLength(128)
              .HasConversion(new EnumToStringConverter<ItemzSeverity>())
              .HasDefaultValue((ItemzSeverity)Enum.Parse(
                                    typeof(ItemzSeverity),
                                    EntityPropertyDefaultValues.ItemzSeverityDefaultValue,
                                    true));

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
                    IsSystem = true,
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
                    IsSystem = true,
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
                },
                new ItemzType()
                {
                    Id = Guid.Parse("52ca1dfc-b187-47fc-a379-57af33404b34"),
                    Name = "TESTING - Unique ItemzType Name",
                    Status = "Active",
                    Description = "Used for testing that ItemzType Name remains unique within a given Project",
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

            // EXPLANATION: We are setting relationship between ItemzChangeHistory
            // and Itemz where by we only store ItemzId in the ItemzChangeHistory. 
            // For now, there is not need to store ItemzChangeHistory in the Itemz
            // Entity as we don't want to have Unique ID (Primary Key) for ItemzChangeHistory 
            // table in the Database.

            modelBuilder.Entity<ItemzChangeHistory>()
                .HasOne(ich => ich.Itemz)
                .WithMany()
                .HasForeignKey(ich => ich.ItemzId);

            base.OnModelCreating(modelBuilder);
        }
    }
}