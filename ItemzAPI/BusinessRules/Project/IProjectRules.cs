// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ItemzApp.API.BusinessRules.Project
{
    public interface IProjectRules
    {
        public bool UniqueProjectNameRule(string sourceProjectName, string targetProjectName = null);
    }
}