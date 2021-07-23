// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace ItemzApp.API.BusinessRules.Baseline
{
    public interface IBaselineRules
    {
        public Task<bool> UniqueBaselineNameRuleAsync(System.Guid ProjectId,  string sourceBaselineName, string? targetBaselineName = null);
    }
}
