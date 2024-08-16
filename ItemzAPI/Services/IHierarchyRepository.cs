// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ItemzApp.API.Entities;
using ItemzApp.API.Models;

namespace ItemzApp.API.Services
{
    public interface IHierarchyRepository
    {
        public Task<HierarchyIdRecordDetailsDTO?> GetHierarchyRecordDetailsByID(Guid recordId);

    }
}
