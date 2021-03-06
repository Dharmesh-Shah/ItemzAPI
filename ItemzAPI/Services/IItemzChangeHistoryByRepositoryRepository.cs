﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace ItemzApp.API.Services
{
    public interface IItemzChangeHistoryByRepositoryRepository
    {
        Task<int> TotalNumberOfItemzChangeHistoryByRepositoryAsync();
        Task<int> TotalNumberOfItemzChangeHistoryByRepositoryUptoDateTimeAsync(DateTimeOffset? GetUptoDateTime = null);
    }
}
