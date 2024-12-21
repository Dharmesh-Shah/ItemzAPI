using MudBlazor;
using static MudBlazor.CategoryTypes;

namespace ItemzApp.WebUI.Helper.WrapperClasses
{
	public class ExtendedBreadcrumbItem
	{
		public BreadcrumbItem OriginalItem { get; set; }
		/// <summary>
		/// Indicates if Baseline Hierarchy record is included or excluded
		/// </summary>
		public bool isIncluded { get; set; }

		public ExtendedBreadcrumbItem(string text, string href, string? icon = null, bool isIncluded = true)
		{
			OriginalItem = new BreadcrumbItem ( text: text, href: href,  icon: icon );
			this.isIncluded = isIncluded;
		}
	}

}
