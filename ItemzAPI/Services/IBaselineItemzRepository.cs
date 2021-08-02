// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IBaselineItemzRepository
    {
        public Task<BaselineItemz?> GetBaselineItemzAsync(Guid BaselineItemzId);

        public Task<IEnumerable<BaselineItemz>?> GetBaselineItemzByItemzIdAsync(Guid ItemzId);

        public Task<bool> BaselineItemzExistsAsync(Guid baselineItemzId);

        public Task<int> GetBaselineItemzCountByItemzIdAsync(Guid ItemzId);

        public Task<IEnumerable<BaselineItemz>> GetBaselineItemzsAsync(IEnumerable<Guid> baselineItemzIds);

    }
}
