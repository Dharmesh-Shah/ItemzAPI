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
