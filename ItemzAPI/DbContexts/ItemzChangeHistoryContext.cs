// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using ItemzApp.API.Entities;
using Microsoft.EntityFrameworkCore;


namespace ItemzApp.API.DbContexts
{
    public class ItemzChangeHistoryContext : DbContext
    {
      
        public ItemzChangeHistoryContext(DbContextOptions<ItemzChangeHistoryContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                   // TODO: for some reason, this is always true here. Investigate why
                   // EF Core team has provided this property and what is the real use 
                   // of the same.
            }
        }

        public DbSet<ItemzChangeHistory> ItemzChangeHistory { get; set; }

        public DbSet<ItemzTypeJoinItemz> ItemzTypeJoinItemz { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ItemzChangeHistory>()
                .HasOne(ich => ich.Itemz)
                .WithMany()
                .HasForeignKey(ich => ich.ItemzId);

            // EXPLANATION: Here we are defining a composite key for a join table.
            // it will use ItemzTypeID + Itemz ID as it's composite key.


            modelBuilder.Entity<ItemzTypeJoinItemz>()
                .HasKey(t => new { t.ItemzTypeId, t.ItemzId });

            // TODO: While ItemzContext is the main class that is going to be
            // used for Database Migrations purposes, We should keep all the 
            // details about configuring models in that central place for now.
            // We have included ItemzTypeJoinItemz in this ItemzChangeHistoryContext
            // purely because while saving ItemzChangeHistory entry in the database,
            // it was throwing exception that it violated rule for Foreign Key
            // between Itemz and ItemzTypeJoinItemz. We could try out using following 
            // Ignore option for ItemzTypeJoinItemz for now if this works.

            //modelBuilder.Ignore<ItemzTypeJoinItemz>();


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

            base.OnModelCreating(modelBuilder);
        }
    }
}