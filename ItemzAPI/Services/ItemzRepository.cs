// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.ResourceParameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{

    public class ItemzRepository : IItemzRepository, IDisposable
    {
        private readonly ItemzContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        public ItemzRepository(ItemzContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public Itemz GetItemz(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return _context.Itemzs
                .Where(c => c.Id == ItemzId).AsNoTracking().FirstOrDefault();
        }

        public Itemz GetItemzForUpdating(Guid ItemzId)
        {

            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return _context.Itemzs
                .Where(c => c.Id == ItemzId).FirstOrDefault();
        }

        public IEnumerable<Itemz> GetItemzs(IEnumerable<Guid> itemzIds)
        {

            if (itemzIds == null)
            {
                throw new ArgumentNullException(nameof(itemzIds));
            }

            return _context.Itemzs.AsNoTracking().Where(a => itemzIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();

        }
        public PagedList<Itemz> GetItemzs(ItemzResourceParameter itemzResourceParameter)
        {
            // TODO: Should we check for itemzResourceParameter being null?
            // There are chances that we just want to get all the itemz and
            // consumer of the API might now pass in necessary values for pagging.

            if (itemzResourceParameter == null)
            {
                throw new ArgumentNullException(nameof(itemzResourceParameter));
            }
            try
            {
                if (_context.Itemzs.Count<Itemz>() > 0)
                {
                    var itemzCollection = _context.Itemzs.AsQueryable<Itemz>(); // as IQueryable<Itemz>;

                    if (!string.IsNullOrWhiteSpace(itemzResourceParameter.OrderBy))
                    {
                        var itemzPropertyMappingDictionary =
                                               _propertyMappingService.GetPropertyMapping<Models.GetItemzDTO, Itemz>();

                        itemzCollection = itemzCollection.ApplySort(itemzResourceParameter.OrderBy,
                            itemzPropertyMappingDictionary).AsNoTracking();  
                    }

                    // EXPLANATION: Pagging feature should be implemented at the end 
                    // just before calling ToList. This will make sure that any filtering,
                    // sorting, grouping, etc. that we implement on the data are 
                    // put in place before calling ToList. 

                    return PagedList<Itemz>.Create(itemzCollection,
                        itemzResourceParameter.PageNumber,
                        itemzResourceParameter.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                // TODO: It's not good that we capture Generic Exception and then 
                // return null here. Basically, I wanted to check if we have 
                // itemzs returned from the DB and if it does not then
                // it should simply return null back to the calling function.
                // One has to learn how to do this gracefully as part of Entity Framework 
                return null;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }

        public void AddItemz(Itemz itemz)
        {
            if (itemz == null)
            {
                throw new ArgumentNullException(nameof(itemz));
            }

            _context.Itemzs.Add(itemz);
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool ItemzExists(Guid itemzId)
        {
            if (itemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(itemzId));
            }

            // EXPLANATION: We expect ItemzExists to be used independently on it's own without
            // expecting it to track the itemz that was found in the database. That's why it's not
            // a good idea to use "!(_context.Itemzs.Find(itemzId) == null)" option
            // to "Find()" Itemz. This is because Find is designed to track the itemz in the memory.
            // In "Itemz Delete controller method", we are first checking if ItemzExists and then 
            // we call Itemz Delete to actually remove it. This is going to be in the single scoped
            // DBContext. If we use "Find()" method then it will start tracking the itemz and then we can't
            // get the itemz once again from the DB as it's already being tracked. We have a choice here
            // to decide if we should always use Find via ItemzExists and then yet again in the subsequent
            // operations like Delete / Update or we use ItemzExists as independent method and not rely on 
            // it for subsequent operations like Delete / Update.

            return _context.Itemzs.AsNoTracking().Any(a => a.Id == itemzId);
            // return  !(_context.Itemzs.Find(itemzId) == null);
        }

        public void UpdateItemz(Itemz itemz)
        {
            // Due to Repository Pattern implementation, 
            // there is no code in this implementation.  
        }

        public void DeleteItemz(Itemz itemz)
        {
            _context.Itemzs.Remove(itemz);
        }
    }
}
