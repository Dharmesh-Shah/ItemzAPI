﻿// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;


namespace ItemzApp.API.Models.BetweenControllerAndRepository
{
	public class RecordCountAndEnumerable<T>
	{
		/// <summary>
		/// Total Number of records in AllRecords
		/// </summary>
		public int RecordCount { get; set; } = 0;

		/// <summary>
		/// AllRecords containing list of nested objects that has several levels of children. 
		/// </summary>
		public IEnumerable<T?> AllRecords = [];
	}
}

