﻿using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API
{
    public class InsertRepositoryEntryInDatabase
    {
        private readonly ItemzContext _itemzContext;
        public InsertRepositoryEntryInDatabase(ItemzContext itemzContext) 
        {
            _itemzContext = itemzContext;   
        }

        private bool RepositoryEntryExists()
        {
            return _itemzContext.ItemzHierarchy!.Any(bih => bih.RecordType == "Repository");
        }

        public void EnsureRepositoryEntryInDatabaseExists()
        {
            if (!(RepositoryEntryExists()))
            {
                // Create a new record and add it to the DbSet
                var newRecord = new ItemzHierarchy
                {
                    Id = Guid.NewGuid(),
                    RecordType = "Repository",
                    ItemzHierarchyId = HierarchyId.Parse("/")
                };

                _itemzContext.ItemzHierarchy!.Add(newRecord);
                _itemzContext.SaveChanges();
            }
        }
    }
}
