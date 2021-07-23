// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;

namespace ItemzApp.API.Services
{
    public interface IBaselineItemzTypeRepository
    {
        public Task<BaselineItemzType?> GetBaselineItemzTypeAsync(Guid BaselineItemzTypeId);

        //public Task<Baseline?> GetBaselineForUpdateAsync(Guid BaselineId);

        public Task<IEnumerable<BaselineItemzType>?> GetBaselineItemzTypesAsync();
        
        //public Task<IEnumerable<BaselineItemzType>?> GetBaselineItemzTypesAsync(Guid BaselineId);
        
        public Task<IEnumerable<BaselineItemzType>> GetBaselineItemzTypesAsync(IEnumerable<Guid> baselineItemzTypeIds);

        //public Task AddBaselineAsync(Baseline baseline);

        //public Task<bool> SaveAsync();
        
        public Task<bool> BaselineItemzTypeExistsAsync(Guid baselineItemzTypeId);

        //public void UpdateBaseline(Baseline baseline);

        //public void DeleteBaseline(Baseline baseline);

        Task<int> GetBaselineItemzCountByBaselineItemzTypeAsync(Guid BaselineItemzTypeId);

        public Task<bool> HasBaselineItemzTypeWithNameAsync(Guid baselineId, string baselineItemzTypeName);

        //public Task<bool> ProjectExistsAsync(Guid projectId);
    }
}
