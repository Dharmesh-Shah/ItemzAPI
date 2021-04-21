using ItemzApp.API.DbContexts;
using ItemzApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{
    public class ItemzChangeHistoryRepository : IItemzChangeHistoryRepository, IDisposable
    {
        private readonly ItemzChangeHistoryContext _context;

        public ItemzChangeHistoryRepository(ItemzChangeHistoryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
        }

        public async Task<IEnumerable<ItemzChangeHistory>> GetItemzChangeHistoryAsync(Guid ItemzId)
        {
            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return await _context.ItemzChangeHistory
                .Where(ich => ich.ItemzId == ItemzId)
                .OrderByDescending(ich => ich.CreatedDate).AsNoTracking().ToListAsync();
        }

        public async Task DeleteItemzChangeHistoryAsync(Guid ItemzId, DateTimeOffset? DeleteUptoDateTime = null)
        {
            if (DeleteUptoDateTime == null)
            {
                return;
            }
            var numberOfItemzChangeHistoryToBeRemoved = 0;
            var foundItemzChangeHistory = _context.ItemzChangeHistory.Where(ich => ich.ItemzId == ItemzId);
            if (foundItemzChangeHistory.Count() > 0)
            {
                foreach (var each_fich in foundItemzChangeHistory)
                {
                    if (each_fich.CreatedDate < DeleteUptoDateTime)
                    {
                        _context.Remove(each_fich);
                        numberOfItemzChangeHistoryToBeRemoved += 1;
                    }
                }
            }
            if(numberOfItemzChangeHistoryToBeRemoved > 0)
            {
                await _context.SaveChangesAsync();
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

    }
}
