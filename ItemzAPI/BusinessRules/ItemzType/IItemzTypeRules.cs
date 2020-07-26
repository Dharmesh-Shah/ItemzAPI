// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace ItemzApp.API.BusinessRules.ItemzType
{
    public interface IItemzTypeRules
    {
        public bool UniqueItemzTypeNameRule(System.Guid ProjectId,  string sourceItemzTypeName, string targetItemzTypeName = null);
    }
}