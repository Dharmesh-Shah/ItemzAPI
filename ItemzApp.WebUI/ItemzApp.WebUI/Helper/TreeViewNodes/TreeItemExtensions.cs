using MudBlazor;

namespace ItemzApp.WebUI.Helper.TreeViewNodes
{
    public static class TreeItemExtensions
    {
        public static IEnumerable<TreeItemData<Guid>> Flatten(this IEnumerable<TreeItemData<Guid>> items)
        {
            foreach (var item in items)
            {
                yield return item;
                if (item.Children != null)
                {
                    foreach (var child in item.Children.Flatten())
                    {
                        yield return child;
                    }
                }
            }
        }
    }

}
