// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IBaselineRepository
    {
        public Task<Baseline?> GetBaselineAsync(Guid BaselineId);

        public Task<Baseline?> GetBaselineForUpdateAsync(Guid BaselineId);

        public Task<IEnumerable<Baseline>?> GetBaselinesAsync();
        
        public Task<IEnumerable<BaselineItemzType>?> GetBaselineItemzTypesAsync(Guid BaselineId);
        
        public Task<IEnumerable<Baseline>> GetBaselinesAsync(IEnumerable<Guid> baselineIds);

        public Task AddBaseline(Baseline baseline);

        public Task<bool> SaveAsync();
        
        public Task<bool> BaselineExistsAsync(Guid baselineId);

        public void UpdateBaseline(Baseline baseline);

        public void DeleteBaseline(Baseline baseline);

        Task<int> GetItemzCountByBaselineAsync(Guid BaselineId);

        public Task<bool> HasBaselineWithNameAsync(string baselineName);
    }
}
