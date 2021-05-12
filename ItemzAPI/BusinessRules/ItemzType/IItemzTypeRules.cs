// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

# nullable enable

namespace ItemzApp.API.BusinessRules.ItemzType
{
    public interface IItemzTypeRules
    {
        public Task<bool> UniqueItemzTypeNameRuleAsync(System.Guid ProjectId,  string sourceItemzTypeName, string? targetItemzTypeName = null);
    }
}

#nullable disable