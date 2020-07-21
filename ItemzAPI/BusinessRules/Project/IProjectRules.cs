namespace ItemzApp.API.BusinessRules.Project
{
    public interface IProjectRules
    {
        public bool UniqueProjectNameRule(string sourceProjectName, string targetProjectName = null);
    }
}