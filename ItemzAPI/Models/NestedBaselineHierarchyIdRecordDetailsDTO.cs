// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace ItemzApp.API.Models
{
    public class NestedBaselineHierarchyIdRecordDetailsDTO
    {
        /// <summary>
        /// Record ID representated by a GUID.
        /// </summary>
        public Guid RecordId { get; set; }

        /// <summary>
        /// Hierarchy ID in string format for RecordId e.g. "/3/2/1"
        /// </summary>
        public string? BaselineHierarchyId { get; set; }

        /// <summary>
        /// Hierarchy Level for RecordId
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// Record Type within Hierarchy for RecordId
        /// </summary>
        public string? RecordType { get; set; }

		/// <summary>
		/// Name of the Hierarchy Record
		/// </summary>
		public string? Name { get; set; }

        public List<NestedBaselineHierarchyIdRecordDetailsDTO>? Children { get; set; }

	}
}
