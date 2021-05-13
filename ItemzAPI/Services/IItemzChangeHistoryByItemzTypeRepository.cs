// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

#nullable enable

namespace ItemzApp.API.Services
{
    public interface IItemzChangeHistoryByItemzTypeRepository
    {
        Task<int> DeleteItemzChangeHistoryByItemzTypeAsync(Guid ItemzTypeId, DateTimeOffset? DeleteUptoDateTime = null);
        Task<int> TotalNumberOfItemzChangeHistoryByItemzTypeAsync(Guid ItemzTypeId);
        Task<int> TotalNumberOfItemzChangeHistoryByItemzTypeUptoDateTimeAsync(Guid ItemzTypeId, DateTimeOffset? GetUptoDateTime = null);

    }
}

#nullable disable
