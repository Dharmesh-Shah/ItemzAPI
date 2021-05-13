// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

#nullable enable

namespace ItemzApp.API.Services
{
    public interface IItemzChangeHistoryByProjectRepository
    {
        Task<int> DeleteItemzChangeHistoryByProjectAsync(Guid ProjectId, DateTimeOffset? DeleteUptoDateTime = null);
        Task<int> TotalNumberOfItemzChangeHistoryByProjectAsync(Guid ProjectId);
        Task<int> TotalNumberOfItemzChangeHistoryByProjectUptoDateTimeAsync(Guid ProjectId, DateTimeOffset? GetUptoDateTime = null);

    }
}

#nullable disable
