
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ItemzApp.WebUI.Client.SharedModels.BetweenPagesAndComponent
{
	public class ParameterForItemzBreadcrums
	{
		/// <summary>
		/// Id of the ItemzType representated by a GUID.
		/// </summary>
		public Guid Id { get; set; }
		/// <summary>
		/// Name or Title of the ItemzType
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Name or Title of the Itemz
		/// </summary>
		public string RecordType { get; set; }
	}
}


