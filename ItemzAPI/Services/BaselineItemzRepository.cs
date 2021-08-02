// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ItemzApp.API.DbContexts;
using ItemzApp.API.DbContexts.Extensions;
using ItemzApp.API.DbContexts.SQLHelper;
using ItemzApp.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{
    public class BaselineItemzRepository : IBaselineItemzRepository, IDisposable
    {
        private readonly BaselineContext _baselineContext;

        public BaselineItemzRepository(BaselineContext baselineContext)
        {
            _baselineContext = baselineContext ?? throw new ArgumentNullException(nameof(baselineContext));
        }
        public async Task<BaselineItemz?> GetBaselineItemzAsync(Guid BaselineItemzId)
        {
            if (BaselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(BaselineItemzId));
            }

            return await _baselineContext.BaselineItemz!
                .Where(bi => bi.Id == BaselineItemzId).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BaselineItemz>?> GetBaselineItemzByItemzIdAsync(Guid ItemzId)
        {
            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }

            return await _baselineContext.BaselineItemz.AsNoTracking().Where(bi => bi.ItemzId == ItemzId)
                .OrderBy(bi => bi.Name)
                .ToListAsync();
        }

        public async Task<bool> BaselineItemzExistsAsync(Guid baselineItemzId)
        {
            if (baselineItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(baselineItemzId));
            }
            return await _baselineContext.BaselineItemz.AsNoTracking().AnyAsync(bi => bi.Id == baselineItemzId);
        }

        public async Task<int> GetBaselineItemzCountByItemzIdAsync(Guid ItemzId)
        {
            if (ItemzId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(ItemzId));
            }
            KeyValuePair<string, object>[] sqlArgs = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("@__ItemzId__", ItemzId.ToString()),
            };
            var foundBaselineItemzByItemzId = await _baselineContext.CountByRawSqlAsync(SQLStatements.SQLStatementFor_GetBaselineItemzByItemzId, sqlArgs);

            return foundBaselineItemzByItemzId;
        }

        public async Task<IEnumerable<BaselineItemz>> GetBaselineItemzsAsync(IEnumerable<Guid> baselineItemzIds)
        {
            if (baselineItemzIds == null)
            {
                throw new ArgumentNullException(nameof(baselineItemzIds));
            }

            return await _baselineContext.BaselineItemz.AsNoTracking().Where(bi => baselineItemzIds.Contains(bi.Id))
                .OrderBy(bi => bi.Name)
                .ToListAsync();
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
