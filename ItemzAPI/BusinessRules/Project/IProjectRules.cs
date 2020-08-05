// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace ItemzApp.API.BusinessRules.Project
{
    public interface IProjectRules
    {
        public Task<bool> UniqueProjectNameRuleAsync(string sourceProjectName, string targetProjectName = null);
    }
}