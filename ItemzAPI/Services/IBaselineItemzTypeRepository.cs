﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using ItemzApp.API.Helper;
using ItemzApp.API.ResourceParameters;

namespace ItemzApp.API.Services
{
    public interface IBaselineItemzTypeRepository
    {
        public Task<BaselineItemzType?> GetBaselineItemzTypeAsync(Guid BaselineItemzTypeId);

        public Task<IEnumerable<BaselineItemzType>?> GetBaselineItemzTypesAsync();
       
        public Task<IEnumerable<BaselineItemzType>> GetBaselineItemzTypesAsync(IEnumerable<Guid> baselineItemzTypeIds);
        
        public Task<bool> BaselineItemzTypeExistsAsync(Guid baselineItemzTypeId);

        Task<int> GetBaselineItemzCountByBaselineItemzTypeAsync(Guid BaselineItemzTypeId);

        PagedList<BaselineItemz>? GetBaselineItemzsByBaselineItemzType(Guid baselineItemzTypeId, ItemzResourceParameter itemzResourceParameter);

        public Task<bool> HasBaselineItemzTypeWithNameAsync(Guid baselineId, string baselineItemzTypeName);

    }
}
